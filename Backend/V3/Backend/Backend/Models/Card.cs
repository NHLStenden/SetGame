

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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
    

    public class Card : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Shape Shape { get; set; }
        [Required]
        public Fill Fill { get; set; }
        [Required]
        public Color Color { get; set; }
        [Required]
        public int NrOfShapes { get; set; }
        
        public ICollection<DeckGameCard> Deck { get; set; }
        
        public ICollection<TableGameCard> OnTable { get; set; }
    }
}