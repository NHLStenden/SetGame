using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Player : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required, MinLength(2), MaxLength(50)]
        public string Name { get; set; }
    }
}