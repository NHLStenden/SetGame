using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Backend.Models
{
    public class Deck : IEntity
    {
        public int Id { get; set; }
        
        //added extra/dummy property, mysql doesn't like tables with only a primary key 
        public string Name { get; set; }
        
        public IList<Card> Cards { get; set; }
    }
}