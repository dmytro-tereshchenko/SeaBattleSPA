using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public class StartField : IStartField
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Points { get; set; }

        [Required]
        public int GameId { get; set; }

        [Required]
        public int GameFieldId { get; set; }

        public int? GamePlayerId { get; set; }

        public Game Game { get; set; }

        [ForeignKey(nameof(GameFieldId))]
        public GameField GameField { get; set; }

        public GamePlayer GamePlayer { get; set; }

        public ICollection<StartFieldCell> StartFieldCells { get; set; }

        public ICollection<GameShip> GameShips { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public StartField()
        {
            StartFieldCells = new List<StartFieldCell>();
            GameShips = new List<GameShip>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="StartField"/> class
        /// </summary>
        /// <param name="id">Id of start field</param>
        /// <param name="field">Game field</param>
        /// <param name="fieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="gamePlayer">Player who own this <see cref="StartField"/></param>
        /// <param name="points">Points for buying ships</param>
        /// <param name="gameShips">Collection of ships that bought but don't put to the field</param>
        /// <param name="gameId">Id of game <see cref="Game"/></param>
        public StartField(int id, GameField field, ICollection<StartFieldCell> fieldLabels, GamePlayer gamePlayer, int points, ICollection<GameShip> gameShips, int gameId)
        : this(field, fieldLabels, gamePlayer, points, gameShips, gameId) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="StartField"/> class
        /// </summary>
        /// <param name="field">Game field</param>
        /// <param name="fieldLabels">Array labels for game field when the player can put his own ships on start field</param>
        /// <param name="gamePlayer">Player who own this <see cref="StartField"/></param>
        /// <param name="points">Points for buying ships</param>
        /// <param name="gameShips">Collection of ships that bought but don't put to the field</param>
        /// <param name="gameId">Id of game <see cref="Game"/></param>
        public StartField(GameField field, ICollection<StartFieldCell> fieldLabels, GamePlayer gamePlayer, int points, ICollection<GameShip> gameShips, int gameId) 
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
        /// <param name="gameId">Id of game <see cref="Game"/></param>
        public StartField(GameField field, int points, int gameId) : this()
        {
            GameField = field;
            Points = points;
            GameId = gameId;
        }
    }
}
