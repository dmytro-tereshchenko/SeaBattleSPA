using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IGame
    {
        uint Id { get; }
        uint FieldId { get; set; }
        byte NumberOfTeam { get; set; }
        string CurrentTeamMove { get; set; }
        IDictionary<string, bool> ReadyTeams { get; }
        IDictionary<string, bool> GivenStartFields { get; }
        bool EndGame { get; set; }
        ICollection<string> Teams { get; }
        IDictionary<string, uint> StartFieldsId { get; }
        bool SearchPlayers { get; set; }
    }
}
