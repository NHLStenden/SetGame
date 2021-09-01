using System.Collections.Generic;
using System.Linq;
using Backend.Models;

namespace Backend.Services
{
    public class PlayerService
    {
        private static List<Player> Players = new List<Player>();

        public Player GetById(int playerId)
        {
            return Players.First(x => x.PlayerId == playerId);
        }

        public Player CreatePlayer(string name)
        {
            var player = new Player()
            {
                Name = name,
                PlayerId = Players.Count + 1
            };
                
            Players.Add(player);
            
            return player;
        }
        
        
    }
}