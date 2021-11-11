using System;
using System.ComponentModel.DataAnnotations;
using Backend.Models;

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

        public Player ToPlayer()
        {
            return new Player()
            {
                Name = Name,
                Email = Email
            };
        }
        
        public PlayerCreateDto CloneWith(Action<PlayerCreateDto> changes)
        {
            var clone = MemberwiseClone() as PlayerCreateDto;

            changes(clone);
            
            return clone;
        }
    }
}