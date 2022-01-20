using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    internal class StartField : IStartField, IEntity
    {
        private uint _id;

        public uint Id
        {
            get => _id;
            private set => _id = value;
        }

        public IGameField GameField { get; set; }

        //labels for gamefield when the player can put his own ships on start field (true - can; false - can't)
        public bool[,] FieldLabels { get; set; }
       
        public string Team { get; set; }

        //points for buying ships
        public int Points { get; set; }

        //ships that bought but don't put to the field
        public ICollection<IGameShip> Ships { get; set; }
        
        public StartField(uint id, IGameField field, bool[,] fieldLabels, string team, int points, ICollection<IGameShip> gameShips)
        :this(field, fieldLabels, team,  points,  gameShips)
        {
            _id = id;
        }

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
