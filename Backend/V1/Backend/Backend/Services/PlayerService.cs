using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Services
{   
    public class PlayerService
    {
        private static List<Player> _players = new List<Player>();

        public Player GetById(int playerId)
        {
            return _players.First(x => x.PlayerId == playerId);
        }

        public Player CreatePlayer(string name)
        {
            var player = new Player()
            {
                Name = name,
                PlayerId = _players.Count + 1
            };
                
            _players.Add(player);
            
            return player;
        }
        
        
    }
}