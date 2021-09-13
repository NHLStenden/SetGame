using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Player
    {
        [Key]
        public int PlayerId { get; set; }
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }
    }
}