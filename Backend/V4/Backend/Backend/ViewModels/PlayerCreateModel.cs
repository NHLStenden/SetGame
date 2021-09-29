using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.Controllers
{
    public class PlayerCreateModel 
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, EmailAddress, Compare(nameof(Email))]
        public string EmailValidate { get; set; }
        
        public PlayerCreateModel CloneWith(Action<PlayerCreateModel> changes)
        {
            var clone = MemberwiseClone() as PlayerCreateModel;

            changes(clone);
            
            return clone;
        }
    }
}