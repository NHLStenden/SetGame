using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class ApiUser : IdentityUser
    {
        public string Name { get; set; }
    }
}