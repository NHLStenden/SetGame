using System;
using System.ComponentModel.DataAnnotations;

namespace Backend.ViewModels
{
    public class PlayerCreateViewModel 
    {
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }
        
        [Required, EmailAddress, Compare(nameof(Email))]
        public string EmailValidate { get; set; }
        
        public PlayerCreateViewModel CloneWith(Action<PlayerCreateViewModel> changes)
        {
            var clone = MemberwiseClone() as PlayerCreateViewModel;

            changes(clone);
            
            return clone;
        }
    }
}