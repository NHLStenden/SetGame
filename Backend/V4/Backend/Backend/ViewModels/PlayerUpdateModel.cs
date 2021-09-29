using System.ComponentModel.DataAnnotations;
using Backend.Controllers;

namespace Backend.ViewModels
{
    public class PlayerUpdateModel : PlayerCreateModel
    {
        [Required]
        public int Id { get; set; }
    }
}