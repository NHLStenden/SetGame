

using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{

    public class Game
    {
        [Key]
        public int GameId { get; set; }
        
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