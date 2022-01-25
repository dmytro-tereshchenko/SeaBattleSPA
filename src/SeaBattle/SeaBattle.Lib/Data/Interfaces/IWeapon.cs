namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Weapon equipment for ship
    /// </summary>
    public interface IWeapon : IEntity
    {
        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Damage { get; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort AttackRange { get; }
    }
}
