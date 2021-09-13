using Dapper.Contrib.Extensions;

namespace Backend.Models
{
    public enum Shape
    {
        Diamond, Pill, Wave
    }

    public enum Fill
    {
        Solid, Hollow, Striped
    }
        
    public enum Color
    {
        Red, Green, Violet
    }
    
    [Table("card")]
    public class Card
    {
        [Key]
        public int CardId { get; set; }
        
        public int DeckId { get; set; }
        
        public Shape Shape { get; set; }
        public Fill Fill { get; set; }
        public Color Color { get; set; }
        public int NrOfShapes { get; set; }
        public int DeckIndex { get; set; }
    }
}