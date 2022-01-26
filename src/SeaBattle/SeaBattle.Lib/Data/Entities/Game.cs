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

        public ICollection<IStartField> StartFields { get; set; }

        public ICollection<IGamePlayer> Players { get; set; }

        public Game(uint id): this() => Id = id;

        public Game(byte maxNumberOfPlayers) : this() => MaxNumberOfPlayers = maxNumberOfPlayers;

        public Game()
        {
            Players = new List<IGamePlayer>();
            State = GameState.Created;
        }
    }
}
