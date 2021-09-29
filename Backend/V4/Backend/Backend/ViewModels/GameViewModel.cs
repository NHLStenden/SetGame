using System.Collections.Generic;
using Backend.Models;

namespace Backend.ViewModels
{
    public class GameViewModel
    {
        public int Id { get; set; }
        
        public int Complexity { get; set; }
        
        public int PlayerId { get; set; }
        public string PlayerName { get; set; }
        
        //Todo Refactor to CardViewModel
        public IList<Card> CardsOnTable { get; set; }
    }
    
    
}