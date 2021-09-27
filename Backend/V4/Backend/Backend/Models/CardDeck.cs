using YamlDotNet.Serialization;

namespace Backend.Models
{
    public class CardDeck
    {
        [YamlIgnore]
        public int DeckId { get; set; }
        public Deck Deck { get; set; }
        
        [YamlIgnore]
        public int CardId { get; set; }
        public Card Card { get; set; }

        public int Order { get; set; }
    }
}