using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    internal class StartField : IStartField, IEntity
    {
        public uint Id { get; set; }

        public IGameField GameField { get; set; }

        /// <summary>
        /// Array labels for game field when the player can put his own ships on start field.
        /// </summary>
        /// <value>Array, when element = true - can put ship ; false - can't</value>
        public bool[,] FieldLabels { get; set; }
       
        public string Team { get; set; }

        /// <summary>
        /// Points for buying ships
        /// </summary>
        public int Points { get; set; }

        /// <summary>
        /// Collection of ships that bought but don't put to the field
        /// </summary>
        public ICollection<IGameShip> Ships { get; set; }
        
        public StartField(uint id, IGameField field, bool[,] fieldLabels, string team, int points, ICollection<IGameShip> gameShips)
        :this(field, fieldLabels, team,  points,  gameShips) => Id = id;

        public StartField(IGameField field, bool[,] fieldLabels, string team, int points, ICollection<IGameShip> gameShips) 
            : this(field, team, points)
        {
            FieldLabels = fieldLabels;
            Ships = gameShips;
        }

        public StartField(IGameField field, string team, int points)
        {
            GameField = field;
            FieldLabels = new bool[field.SizeX, field.SizeY];
            Team = team;
            Points = points;
            Ships = new List<IGameShip>();
        }
    }
}
