using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// The field for storing the location of ships and points for buy ships by the player when initializing game.
    /// </summary>
    public class StartField : IStartField
    {
        public uint Id { get; set; }

        public uint GameId { get; set; }

        public IGameField GameField { get; set; }

        public bool[,] FieldLabels { get; set; }

        public IGamePlayer GamePlayer { get; set; }

        public int Points { get; set; }

        public ICollection<IGameShip> Ships { get; set; }
        
        public StartField(uint id, IGameField field, bool[,] fieldLabels, IGamePlayer gamePlayer, int points, ICollection<IGameShip> gameShips, uint gameId)
        :this(field, fieldLabels, gamePlayer,  points,  gameShips, gameId) => Id = id;

        public StartField(IGameField field, bool[,] fieldLabels, IGamePlayer gamePlayer, int points, ICollection<IGameShip> gameShips, uint gameId) 
            : this(field, points, gameId)
        {
            FieldLabels = fieldLabels;
            Ships = gameShips;
            GamePlayer = gamePlayer;
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
