using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class LoginUserDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}