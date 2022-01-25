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
        InvalidPlayer = 014,

        //Null reference instead of data
        NullReference = 015,

        //Exceeded max number of players
        ExceededMaxNumberOfPlayers = 016
    }
}
