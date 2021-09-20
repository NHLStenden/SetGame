namespace Backend.Models
{
    public class DeckGameCard
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        public int Order { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
    
    public class TableGameCard
    {
        public int GameId { get; set; }
        public Game Game { get; set; }
        
        public int Order { get; set; }

        public int CardId { get; set; }
        public Card Card { get; set; }
    }
}