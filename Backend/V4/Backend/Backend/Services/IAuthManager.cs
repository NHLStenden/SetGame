using System.Threading.Tasks;
using Backend.DTOs;

namespace Backend.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDto loginUserDto);
        Task<string> CreateToken();
    }
}