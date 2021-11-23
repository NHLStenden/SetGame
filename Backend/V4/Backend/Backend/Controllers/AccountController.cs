using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Backend.Dtos;
using Backend.DTOs;
using Backend.Extensions;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<Player> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(UserManager<Player> userManager, IMapper mapper, IAuthManager authManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _authManager = authManager;
        }
        
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] PlayerCreateDto playerCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var apiUser = _mapper.Map<Player>(playerCreateDto);
            // apiUser.UserName = playerCreateDto.Email; //handle by automapper see MappingProfile.cs

            var createUserResult = await _userManager.CreateAsync(apiUser, playerCreateDto.Password);
            if (!createUserResult.Succeeded)
            {
                foreach (var error in createUserResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            var addToRolesResult = await _userManager.AddToRolesAsync(apiUser, playerCreateDto.Roles);
            if (!addToRolesResult.Succeeded)
            {
                foreach (var error in addToRolesResult.Errors)
                {
                    ModelState.AddModelError(error.Code, error.Description);
                }

                return BadRequest(ModelState);
            }

            return Accepted();
        }

        [HttpPost("Login")]
        public async Task<ActionResult<AuthenticationResult>> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateUserResult = await _authManager.ValidateUserAsync(loginUserDto);
            if (validateUserResult == null)
                return Unauthorized();

            var token = await _authManager.CreateTokenAsync();
            return Accepted(token);
        }

        [HttpPost("Refresh")]
        public async Task<AuthenticationResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            var authenticationResult = await _authManager.RefreshTokenAsync(request.Token, request.RefreshToken);

            return authenticationResult;
        }

        [HttpGet("DisplayClaims")]
        public async Task<IActionResult> DisplayClaims()
        {
            //long? userId = HttpContext.GetUserId();

            return Ok(HttpContext.User.Claims.Select( x => new {Type = x.Type, Value = x.Value}));
        }

        [Authorize(Roles = "Administrator")]
        [HttpGet(nameof(MethodThatNeedsAdminRole))]
        public async Task<IActionResult> MethodThatNeedsAdminRole()
        {
            var result = await CreateDisplayMessage();
            return Ok(result);
        }

  

        [Authorize(Roles = "Administrator, User")]
        [HttpGet(nameof(MethodThatNeedsAdminRoleOrUserRole))]
        public async Task<IActionResult> MethodThatNeedsAdminRoleOrUserRole()
        {
            var result = await CreateDisplayMessage();
            return Ok(result);
        }
        
        [Authorize]
        [HttpGet(nameof(MethodThatNeedsAuthenticatedUser))]
        public async Task<IActionResult> MethodThatNeedsAuthenticatedUser()
        {
            var result = await CreateDisplayMessage();
            return Ok(result);
        }

        private async Task<string> CreateDisplayMessage()
        {
            var user = await _userManager.FindByIdAsync(HttpContext.GetUserId().ToString());
            var roles = await _userManager.GetRolesAsync(user);
            string result =
                $"Correct Credentials for User with Id: {HttpContext.GetUserId()} With role(s): {String.Join(",", roles)}";
            return result;
        }
    }
}