using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.DTOs;
using Backend.Models;
using Backend.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<Player> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly JwtSettings _jwtSettings;
        private readonly SetContext _db;
        private Player _user;

        public AuthManager(UserManager<Player> userManager,
            TokenValidationParameters tokenValidationParameters, JwtSettings jwtSettings, SetContext db)
        {
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _jwtSettings = jwtSettings;
            _db = db;
        }
        
        public async Task<Player> ValidateUserAsync(LoginUserDto loginUserDto)
        {
            _user = await _userManager.FindByNameAsync(loginUserDto.Email);
            if (_user == null)
                return null;

            var passwordCorrect = await _userManager.CheckPasswordAsync(_user, loginUserDto.Password);
            if (!passwordCorrect)
                return null;

            return _user;
        }

        public async Task<AuthenticationResult> CreateTokenAsync()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            
            //save token in the database, used for refresh of tokens
            var refreshToken = new RefreshToken()
            {
                //Token = this is generated by the database as a GUID 
                JwtId = _user.Id,          //Todo: Not sure if this is correct (will work anyway):  https://stackoverflow.com/questions/28907831/how-to-use-jti-claim-in-a-jwt
                UserId = _user.Id,         //Todo: duplication of JWT remove/or add other value
                CreationDate = DateTime.UtcNow,
                ExpireDate = DateTime.UtcNow.Add(_jwtSettings.RefreshTokenLifeTime)
            };

            //Todo: use repository
            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
            
            return new AuthenticationResult
            {
                Success = true,
                Token = token,
                RefreshToken = refreshToken.Token
            };
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string requestToken, Guid refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(requestToken);
            if (validatedToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Token" }
                };
            }

            var expireDate = long.Parse(
                validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expireDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expireDate);
                // .Subtract(_jwtSettings.TokenLifeTime);

            if (expireDateUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This token hasn't expired yet" }
                };
            }

            //Todo: use repository
            var storedRefreshToken = await _db.RefreshTokens
                .SingleOrDefaultAsync(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Refresh Token does not exists" }
                };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpireDate)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Refresh Token has expired" }
                }; 
            }

            if (storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "This Token has been invalidated" }
                };
            }

            if (storedRefreshToken.Used)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "This Token has been used" }
                };
            }
            
            var jti = 
                int.Parse(
                    validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value);

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult()
                {
                    Errors = new[] { "This Token does not match this JWT" }
                };
            }

            //Todo: use repository
            storedRefreshToken.Used = true;
            _db.RefreshTokens.Update(storedRefreshToken);
            await _db.SaveChangesAsync();

            _user = await _userManager.FindByIdAsync(jti.ToString());

            var authenticationResult = await CreateTokenAsync();
            return authenticationResult;
        }
        
        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
        
        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                claims: claims,
                expires: DateTime.UtcNow.Add(_jwtSettings.TokenLifeTime),
                signingCredentials: signingCredentials
            );
            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName)
            };

            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, _user.Id.ToString()));

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}