using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public interface IGame: IEntity
    {
        uint FieldId { get; set; }

        byte NumberOfTeam { get; }

        string CurrentTeamMove { get; set; }

        IDictionary<uint, bool> GivenStartFields { get; set; }

        bool EndGame { get; set; }

        ICollection<uint> TeamsId { get; set; }

        IDictionary<uint, uint> StartFieldsId { get; set; }

        bool SearchPlayers { get; set; }
    }
}
