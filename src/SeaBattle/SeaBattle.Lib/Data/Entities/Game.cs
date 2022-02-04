using System.Collections.Generic;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public class Game : IGame
    {
        public uint Id { get; set; }

        public IGameField Field { get; set; }

        public IGamePlayer CurrentGamePlayerMove { get; set; }
        
        public byte CurrentCountPlayers
        {
            get => (byte) Players.Count;
        }

        public byte MaxNumberOfPlayers { get; private set; }
        
        public GameState State { get; set; }

        public IGamePlayer Winner { get; set; }

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
            State = GameState.Created;
        }
    }
}
