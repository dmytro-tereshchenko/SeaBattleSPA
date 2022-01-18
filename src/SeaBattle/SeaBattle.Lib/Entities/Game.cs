using System;
using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public class Game : IGame, IEntity
    {
        private uint _id;

        private uint _fieldId;

        private byte _numberOfTeam;

        private string _currentTeamMove;

        private IDictionary<string, bool> _readyTeams;

        private IDictionary<string, bool> _givenStartFields;

        private bool _endGame;

        private ICollection<string> _teams;

        private IDictionary<string, uint> _startFieldsId;

        private bool _searchPlayers;
        public uint Id { get => _id; }
        public uint FieldId { get => _fieldId; set => _fieldId = value; }
        public byte NumberOfTeam { get => _numberOfTeam; set => _numberOfTeam = value; }
        public string CurrentTeamMove { get => _currentTeamMove; set => _currentTeamMove = value; }
        public IDictionary<string, bool> ReadyTeams { get => _readyTeams; }
        public IDictionary<string, bool> GivenStartFields { get => _givenStartFields; }
        public bool EndGame { get => _endGame; set => _endGame = value; }
        public ICollection<string> Teams { get => _teams; }
        public IDictionary<string, uint> StartFieldsId { get => _startFieldsId; }
        public bool SearchPlayers { get => _searchPlayers; set => _searchPlayers = value; }
        public Game(uint id, byte numberOfTeam)
        {
            this._id = id;
            this._numberOfTeam = numberOfTeam;
            _readyTeams = new Dictionary<string, bool>();
            _givenStartFields = new Dictionary<string, bool>();
            _teams = new List<string>();
            _startFieldsId = new Dictionary<string, uint>();
        }
    }
}
