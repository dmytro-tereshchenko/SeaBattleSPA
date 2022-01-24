using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public class StartField : IStartField
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Id of game <see cref="IGame"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint GameId { get; set; }

        /// <summary>
        /// Link to game field
        /// </summary>
        /// <value><see cref="IGameField"/></value>
        public IGameField GameField { get; set; }

        /// <summary>
        /// Array labels for game field when the player can put his own ships on start field.
        /// </summary>
        /// <value>Array <see cref="bool"/>[,], when element = true - can put ship ; false - can't</value>
        public bool[,] FieldLabels { get; set; }

        /// <summary>
        /// Name of player
        /// </summary>
        /// <value><see cref="string"/></value>
        public string Player { get; set; }

        /// <summary>
        /// Player's Id
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Points for buying ships
        /// </summary>
        /// <value><see cref="int"/></value>
        public int Points { get; set; }

        /// <summary>
        /// Collection of ships that bought but don't put to the field
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameShip"</value>
        public ICollection<IGameShip> Ships { get; set; }
        
        public StartField(uint id, IGameField field, bool[,] fieldLabels, string player, int points, ICollection<IGameShip> gameShips, uint gameId)
        :this(field, fieldLabels, player,  points,  gameShips, gameId) => Id = id;

        public StartField(IGameField field, bool[,] fieldLabels, string player, int points, ICollection<IGameShip> gameShips, uint gameId) 
            : this(field, points, gameId)
        {
            FieldLabels = fieldLabels;
            Ships = gameShips;
            Player = player;
        }

        public StartField(IGameField field, int points, uint gameId)
        {
            GameField = field;
            FieldLabels = new bool[field.SizeX, field.SizeY];
            Points = points;
            GameId = gameId;
            Ships = new List<IGameShip>();
        }
    }
}
