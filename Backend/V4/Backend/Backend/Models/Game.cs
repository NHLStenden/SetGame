

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace Backend.Models
{
    public class Game : IEntity
    {
        [Key]
        [YamlIgnore]
        public int Id { get; set; }
        
        [Required]
        [YamlIgnore]
        public int DeckId { get; set; }
        [Required]
        public Deck Deck { get; set; }
        
        [Required]
        public int CardIndex { get; set; }
        
        [Required]
        [YamlIgnore]
        public int PlayerId { get; set; }
        [Required]
        public Player Player { get; set; }
        
        public IList<CardOnTable> CardsOnTable { get; set; }
    }
}