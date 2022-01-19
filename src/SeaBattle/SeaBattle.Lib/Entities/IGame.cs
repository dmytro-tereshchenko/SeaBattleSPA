using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IGame: IEntity
    {
        uint FieldId { get; set; }

        byte NumberOfTeam { get; }

        string CurrentTeamMove { get; set; }

        IDictionary<string, bool> GivenStartFields { get; }

        bool EndGame { get; set; }

        ICollection<uint> TeamsId { get; }

        IDictionary<string, uint> StartFieldsId { get; }

        bool SearchPlayers { get; set; }
    }
}
