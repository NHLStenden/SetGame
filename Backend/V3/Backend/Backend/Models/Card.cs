

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
        public Shape Shape { get; set; }
        [Required]
        public Fill Fill { get; set; }
        [Required]
        public Color Color { get; set; }
        [Required]
        public int NrOfShapes { get; set; }
    }
}