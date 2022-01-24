using System;
using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public class Game : IGame
    {
        public uint Id { get; set; }

        public uint FieldId { get; set; }

        public byte NumberOfTeam { get => Convert.ToByte(TeamsId.Count); }

        public string CurrentTeamMove { get; set; }
        
        public IDictionary<uint, bool> GivenStartFields { get; set; }
        
        public bool EndGame { get; set; }
        
        public ICollection<uint> TeamsId { get; set; }

        /// <summary>
        /// Dictionary of fields to stores data about fields for the allocation ships by players when the game is initializing.
        /// </summary>
        /// <value>Dictionary: Key - teamId, Value - startFieldId</value>
        public IDictionary<uint, uint> StartFieldsId { get; set; }
        
        public bool SearchPlayers { get; set; }
        
        public Game(uint id): this() => Id = id;

        public Game()
        {
            GivenStartFields = new Dictionary<uint, bool>();
            TeamsId = new List<uint>();
            StartFieldsId = new Dictionary<uint, uint>();
        }
    }
}
