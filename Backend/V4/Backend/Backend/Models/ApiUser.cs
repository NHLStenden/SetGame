using Microsoft.AspNetCore.Identity;

namespace Backend.Models
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}