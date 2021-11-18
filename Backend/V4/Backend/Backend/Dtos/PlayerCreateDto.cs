using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class PlayerCreateDto 
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, EmailAddress, Compare(nameof(Email))]
        public string EmailValidate { get; set; }

        [Required, MinLength(8), MaxLength(25)]
        public string Password { get; set; }
        
        [Required, MinLength(8), MaxLength(25), Compare(nameof(Password))]
        public string PasswordValidate { get; set; }
        
        public ICollection<string> Roles { get; set; }
        
        public PlayerCreateDto CloneWith(Action<PlayerCreateDto> changes)
        {
            var clone = MemberwiseClone() as PlayerCreateDto;

            changes(clone);
            
            return clone;
        }
    }
}