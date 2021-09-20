

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models
{
    
    
    public class Game : IEntity
    {
        [Key]
        public int Id { get; set; }
        
        public ICollection<DeckGameCard> Deck { get; set; }
        
        public ICollection<TableGameCard> OnTable { get; set; }
        
        [Required]
        public int CardIndex { get; set; }
        
        [Required]
        public int PlayerId { get; set; }
        [Required]
        public Player Player { get; set; }
    }
}