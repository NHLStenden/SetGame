

using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public int DeckId { get; set; }
        [Required]
        public Deck Deck { get; set; }
        
        [Required]
        public int CardIndex { get; set; }
        
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public Player Player { get; set; }
    }
}