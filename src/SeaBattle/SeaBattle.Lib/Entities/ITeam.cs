using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface ITeam : IEntity
    {

        public string Name { get; }

        public uint GameId { get; }

        public bool Ready { get; set; }
    }
}
