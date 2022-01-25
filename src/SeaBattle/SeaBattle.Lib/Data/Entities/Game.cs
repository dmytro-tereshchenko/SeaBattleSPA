using System;
using System.Collections.Generic;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public class Game : IGame
    {
        /// <summary>
        /// Id of entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Field of the game
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        public IGameField Field { get; set; }

        /// <summary>
        /// Player, which needs to move this turn.
        /// </summary>
        /// <value><see cref="IPlayer"/></value>
        public IPlayer CurrentPlayerMove { get; set; }
        
        /// <summary>
        /// Current amount of players which connected to the game
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte CurrentCountPlayers { get; set; }

        /// <summary>
        /// Max amount of players
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte MaxNumberOfPlayers { get; private set; }
        
        /// <summary>
        /// Current state of game
        /// </summary>
        /// <value><see cref="StateGame"/></value>
        public StateGame State { get; set; }

        /// <summary>
        /// Collection of fields with initializing data and parameters for every player
        /// </summary>
        /// <value><see cref="StateGame"/></value>
        public ICollection<IStartField> StartFields { get; set; }

        /// <summary>
        /// Collection of players for current game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IPlayer"/></value>
        public ICollection<IPlayer> Players { get; set; }

        public Game(uint id): this() => Id = id;

        public Game(byte maxNumberOfPlayers) : this() => MaxNumberOfPlayers = maxNumberOfPlayers;

        public Game()
        {
            Players = new List<IPlayer>();
            State = StateGame.Created;
            CurrentCountPlayers = 0;
        }
    }
}
