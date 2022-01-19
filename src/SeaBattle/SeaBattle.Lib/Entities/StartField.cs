using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    internal class StartField : IStartField, IEntity
    {
        private uint _id;

        public uint Id { get => _id; }

        public IGameField GameField { get; set; }

        //labels for gamefield when the player can put his own ships on start field (true - can; false - can't)
        public bool[,] FieldLabels { get; set; }
       
        public string Team { get; set; }

        //points for buying ships
        public int Points { get; set; }

        //ships that bought but don't put to the field
        public ICollection<IGameShip> Ships { get; set; }
        
        public StartField(uint id, IGameField field, bool[,] fieldLabels, string team, int points, ICollection<IGameShip> gameShips)
        {
            this._id = id;
            this.GameField = field;
            this.FieldLabels = fieldLabels;
            this.Team = team;
            this.Points = points;
            this.Ships = gameShips;
        }
    }
}