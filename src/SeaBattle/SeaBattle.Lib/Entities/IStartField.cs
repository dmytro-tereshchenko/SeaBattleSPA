using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IStartField : IEntity
    {

        IGameField GameField { get; set; }

        //labels for gamefield when the player can put his own ships on start field (true - can; false - can't)
        bool[,] FieldLabels { get; set; }
        
        string Team { get; set; }

        //points for buying ships
        int Points { get; set; }

        //ships that bought but don't put to the field
        ICollection<IGameShip> Ships { get; set; }

    }
}
