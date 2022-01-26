namespace SeaBattle.Lib.Infrastructure
{
    /// <summary>
    /// Types of possible interaction between ships
    /// </summary>
    public enum ActionType
    {
        /// <summary>
        /// Attack target ship (reduce his hp)
        /// </summary>
        Attack,

        /// <summary>
        /// Repair target ship/ships (restore his/them hp)
        /// </summary>
        Repair
    }
}
