using System;
using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public class Game : IGame
    {
        private uint _id;

        private uint _fieldId;

        private string _currentTeamMove;

        private IDictionary<uint, bool> _givenStartFields;

        private bool _endGame;

        private ICollection<uint> _teamsId;

        private IDictionary<uint, uint> _startFieldsId;

        private bool _searchPlayers;

        public uint Id { get => _id; }

        public uint FieldId { get => _fieldId; set => _fieldId = value; }

        public byte NumberOfTeam { get => Convert.ToByte(_teamsId.Count); }

        public string CurrentTeamMove { get => _currentTeamMove; set => _currentTeamMove = value; }
        
        public IDictionary<uint, bool> GivenStartFields { get => _givenStartFields; }
        
        public bool EndGame { get => _endGame; set => _endGame = value; }
        
        public ICollection<uint> TeamsId { get => _teamsId; }
        
        public IDictionary<uint, uint> StartFieldsId { get => _startFieldsId; }
        
        public bool SearchPlayers { get => _searchPlayers; set => _searchPlayers = value; }
        
        public Game(uint id): this()
        {
            _id = id;
        }

        public Game()
        {
            _givenStartFields = new Dictionary<uint, bool>();
            _teamsId = new List<uint>();
            _startFieldsId = new Dictionary<uint, uint>();
        }
    }
}
