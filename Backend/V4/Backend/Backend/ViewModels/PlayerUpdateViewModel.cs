using System.ComponentModel.DataAnnotations;
using Backend.Controllers;

namespace Backend.ViewModels
{
    public class PlayerUpdateViewModel : PlayerCreateViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}