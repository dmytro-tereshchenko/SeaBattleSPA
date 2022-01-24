namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Enum possible states and Errors in the domain.
    /// </summary>
    public enum StateCode
    {
        //OK
        Success = 010,

        //Not enough allocation points for buying the ship
        PointsShortage = 011,

        //Invalid cell for the placement ship
        InvalidPositionShip = 012,

        //Invalid playing field size
        InvalidFieldSize = 013,

        //Invalid Team, the team isn't matched with the field
        InvalidTeam = 014,

        //Invalid id for data in a repository
        InvalidId = 015
    }
}
