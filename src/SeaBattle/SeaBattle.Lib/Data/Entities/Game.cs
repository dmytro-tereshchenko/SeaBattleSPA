using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public class Game : IGame
    {
        public uint Id { get; set; }

        [JsonIgnore]
        public uint GameFieldId { get; set; }

        public string CurrentGamePlayerMove { get; set; }
        
        [NotMapped]
        public byte CurrentCountPlayers
        {
            get => (byte) Players.Count;
        }

        public byte MaxNumberOfPlayers { get; set; }

        [JsonIgnore]
        public uint GameStateId { get; set; }

        public string Winner { get; set; }

        [ForeignKey("GameFieldId")]
        public IGameField GameField { get; set; }

        [ForeignKey("GameStateId")]
        public IGameState GameState { get; set; }

        public ICollection<IStartField> StartFields { get; set; }

        public ICollection<IGamePlayer> Players { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        /// <param name="id">Id of game</param>
        public Game(uint id): this() => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        /// <param name="maxNumberOfPlayers">Max amount of players</param>
        public Game(byte maxNumberOfPlayers) : this() => MaxNumberOfPlayers = maxNumberOfPlayers;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        public Game()
        {
            Players = new List<IGamePlayer>();
            StartFields = new List<IStartField>();
        }
    }
}
