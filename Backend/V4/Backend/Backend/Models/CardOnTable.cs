namespace Backend.Models
{
    public class CardOnTable
    {
        public int CardId { get; set; }
        public Card Card { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public int Order { get; set; }
    }
}