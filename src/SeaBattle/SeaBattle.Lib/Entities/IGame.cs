using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public interface IGame: IEntity
    {
        uint FieldId { get; set; }

        byte NumberOfTeam { get; }

        string CurrentTeamMove { get; set; }

        IDictionary<uint, bool> GivenStartFields { get; }

        bool EndGame { get; set; }

        ICollection<uint> TeamsId { get; }

        IDictionary<uint, uint> StartFieldsId { get; }

        bool SearchPlayers { get; set; }
    }
}
