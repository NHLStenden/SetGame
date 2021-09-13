using System.Collections.Generic;
using Dapper.Contrib.Extensions;

namespace Backend.Models
{
    [Table("Deck")]
    public class Deck
    {
        [Key]
        public int DeckId { get; set; }
        [Write(false)]
        public Card[] Cards { get; set; }
    }
}