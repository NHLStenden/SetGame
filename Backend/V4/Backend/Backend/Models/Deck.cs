using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YamlDotNet.Serialization;

namespace Backend.Models
{
    public class Deck : IEntity
    {
        [YamlIgnore]
        public int Id { get; set; }
        
        //added extra/dummy property, mysql doesn't like tables with only a primary key 
        public int Complexity { get; set; }
        
        public IList<CardDeck> Cards { get; set; }
    }
}