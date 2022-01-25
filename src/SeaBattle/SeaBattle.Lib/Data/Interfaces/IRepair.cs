namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Repair equipment for ship
    /// </summary>
    public interface IRepair : IEntity
    {
        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairPower { get; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairRange { get; }
    }
}
