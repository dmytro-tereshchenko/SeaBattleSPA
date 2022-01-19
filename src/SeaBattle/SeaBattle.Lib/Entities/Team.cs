using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class Team : ITeam
    {
       
        private uint _id;

        private string _name;

        private uint _gameId;

        public uint Id { get => _id; }

        public string Name { get => _name; }

        public uint GameId { get => _gameId; }

        public Team(uint id, string name, uint gameId)
        {
            _id = id;
            _name = name;
            _gameId = gameId;
        }
    }
}
