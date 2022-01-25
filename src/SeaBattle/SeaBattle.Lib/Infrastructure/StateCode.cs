namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Enum possible states and errors in the domain.
    /// </summary>
    public enum StateCode
    {
        /// <summary>
        /// OK
        /// </summary>
        Success = 010,

        /// <summary>
        /// Not enough allocation points for buying the ship
        /// </summary>
        PointsShortage = 011,

        /// <summary>
        /// Invalid cell for the placement ship
        /// </summary>
        InvalidPositionShip = 012,

        /// <summary>
        /// Invalid playing field size
        /// </summary>
        InvalidFieldSize = 013,

        /// <summary>
        /// Invalid Team, the team isn't matched with the field
        /// </summary>
        InvalidPlayer = 014,

        /// <summary>
        /// Null reference instead of data
        /// </summary>
        NullReference = 015,

        /// <summary>
        /// Exceeded max number of players
        /// </summary>
        ExceededMaxNumberOfPlayers = 016,

        /// <summary>
        /// Invalid place for ship (there is around another ship or out of game field
        /// </summary>
        InvalidPlaceForShip = 020
    }
}
