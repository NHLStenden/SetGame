

using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

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
        [YamlIgnore]
        public int Id { get; set; }

        [Required]
        public Shape Shape { get; init; }
        [Required]
        public Fill Fill { get; init; }
        [Required]
        public Color Color { get; init; }
        [Required]
        public int NrOfShapes { get; init; }
    }
}