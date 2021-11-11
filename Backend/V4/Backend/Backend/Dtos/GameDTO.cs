using System.Collections.Generic;
using Backend.Models;

namespace Backend.DTOs
{
    public class GameDto
    {
        public int Id { get; set; }
        
        public int Complexity { get; set; }
        
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        
        //Todo Refactor to CardViewModel
        public IList<Card> CardsOnTable { get; set; }
    }
    
    
}