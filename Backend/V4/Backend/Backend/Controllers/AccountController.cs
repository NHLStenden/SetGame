using System.Threading.Tasks;
using AutoMapper;
using Backend.DTOs;
using Backend.Models;
using Backend.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IAuthManager _authManager;

        public AccountController(
            UserManager<ApiUser> userManager, 
            IMapper mapper,
            IAuthManager authManager)
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

            var apiUser = _mapper.Map<ApiUser>(playerCreateDto);
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
        public async Task<IActionResult> Login(LoginUserDto loginUserDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var validateUserResult = await _authManager.ValidateUser(loginUserDto);
            if (!validateUserResult)
                return Unauthorized();

            var token = await _authManager.CreateToken();
            return Accepted(new { Token = token });
        }
    }
}