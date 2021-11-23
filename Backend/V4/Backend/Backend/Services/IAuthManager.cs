using System;
using System.Threading.Tasks;
using Backend.Dtos;
using Backend.DTOs;
using Backend.Models;

namespace Backend.Services
{
    public interface IAuthManager
    {
        Task<Player> ValidateUserAsync(LoginUserDto loginUserDto);
        Task<AuthenticationResult> CreateTokenAsync();
        Task<AuthenticationResult> RefreshTokenAsync(string requestToken, Guid requestRefreshToken);
    }
}