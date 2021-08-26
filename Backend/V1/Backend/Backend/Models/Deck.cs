using System.Collections.Generic;

namespace Backend.Models
{

    public class Deck
    {
        public int DeckId { get; set; }
        public IList<Card> Cards { get; set; }

        private int _drawIndex = 0;

        public Card DrawCard()
        {
            if (_drawIndex < Cards.Count)
            {
                return Cards[_drawIndex++];
            }
            else
            {
                return null;
            }
        }
    }
    

}