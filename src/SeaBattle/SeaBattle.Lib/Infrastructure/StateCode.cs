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
        /// Invalid player, the player isn't matched with the field
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
        /// Invalid ship in start field
        /// </summary>
        InvalidShip = 017,

        /// <summary>
        /// Absence of a ship by coordinates
        /// </summary>
        AbsentOfShip = 020,

        /// <summary>
        /// Target out of distance
        /// </summary>
        OutOfDistance = 021,

        /// <summary>
        /// Action on target was missed
        /// </summary>
        MissTarget = 022,

        /// <summary>
        /// Target was destroyed
        /// </summary>
        TargetDestroyed = 023,

        /// <summary>
        /// Invalid operation
        /// </summary>
        InvalidOperation = 0
    }
}
