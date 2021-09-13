using Dapper.Contrib.Extensions;

namespace Backend.Models
{
    [Table("Game")]
    public class Game
    {
        [Key]
        public int GameId { get; set; }
        
        public int DeckId { get; set; }
        
        
        public Deck Deck { get; set; }
        
        public int CardIndex { get; set; }
        public int PlayerId { get; set; }
        public Player Player { get; set; }
    }
}