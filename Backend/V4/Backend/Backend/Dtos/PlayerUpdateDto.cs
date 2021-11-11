using System.ComponentModel.DataAnnotations;

namespace Backend.DTOs
{
    public class PlayerUpdateDto : PlayerCreateDto
    {
        [Required]
        public int Id { get; set; }
    }
}