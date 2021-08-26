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
    
    public class Card
    {
        public Shape Shape { get; set; }
        public Fill Fill { get; set; }
        public Color Color { get; set; }
        public int NrOfShapes { get; set; }
    }
}