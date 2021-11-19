using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Backend.DTOs;
using Backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<Player> _userManager;
        private readonly IConfiguration _configuration;
        private Player _user;

        public AuthManager(UserManager<Player> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        
        public async Task<bool> ValidateUser(LoginUserDto loginUserDto)
        {
            _user = await _userManager.FindByNameAsync(loginUserDto.Email);
            bool result = _user != null && await _userManager.CheckPasswordAsync(_user, loginUserDto.Password);
            return result;
        }

        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);

            string token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return token;
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"], 
                claims: claims, 
                expires: DateTime.Now.AddMinutes(Int32.Parse(jwtSettings["lifeTime"])),
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

            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var key = jwtSettings["Key"];
            var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

            return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
        }
    }
}