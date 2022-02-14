using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public class StartField : IStartField
    {
        public uint Id { get; set; }

        public uint GameId { get; set; }

        [JsonIgnore]
        public uint GameFieldId { get; set; }

        [JsonIgnore]
        public uint GamePlayerId { get; set; }

        public ICollection<IStartFieldCell> StartFieldCells { get; set; }

        public int Points { get; set; }

        [ForeignKey("GameFieldId")]
        public IGameField GameField { get; set; }

        [ForeignKey("GamePlayerId")]
        public IGamePlayer GamePlayer { get; set; }

        [JsonIgnore]
        [ForeignKey("GameId")]
        public IGame Game { get; set; }

        public StartField()
        {
            StartFieldCells = new List<IStartFieldCell>();
        }

        public ICollection<IGameShip> GameShips { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartField"/> class
        /// </summary>
        /// <param name="id">Id of start field</param>
        /// <param name="field">Game field</param>
        /// <param name="fieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="gamePlayer">Player who own this <see cref="IStartField"/></param>
        /// <param name="points">Points for buying ships</param>
        /// <param name="gameShips">Collection of ships that bought but don't put to the field</param>
        /// <param name="gameId">Id of game <see cref="IGame"/></param>
        public StartField(uint id, IGameField field, ICollection<IStartFieldCell> fieldLabels, IGamePlayer gamePlayer, int points, ICollection<IGameShip> gameShips, uint gameId)
        :this(field, fieldLabels, gamePlayer,  points,  gameShips, gameId) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartField"/> class
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="fieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="gamePlayer">Player who own this <see cref="IStartField"/></param>
        /// <param name="points">Points for buying ships</param>
        /// <param name="gameShips">Collection of ships that bought but don't put to the field</param>
        /// <param name="gameId">Id of game <see cref="IGame"/></param>
        public StartField(IGameField field, ICollection<IStartFieldCell> fieldLabels, IGamePlayer gamePlayer, int points, ICollection<IGameShip> gameShips, uint gameId) 
            : this(field, points, gameId)
        {
            StartFieldCells = fieldLabels;
            GameShips = gameShips;
            GamePlayer = gamePlayer;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartField"/> class
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="points">Points for buying ships</param>
        /// <param name="gameId">Id of game <see cref="IGame"/></param>
        public StartField(IGameField field, int points, uint gameId)
        {
            GameField = field;
            StartFieldCells = new List<IStartFieldCell>();
            Points = points;
            GameId = gameId;
            GameShips = new List<IGameShip>();
        }
    }
}
