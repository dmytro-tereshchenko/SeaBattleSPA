namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic weapon equipment for ship
    /// </summary>
    public class BasicWeapon : IWeapon
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort Damage { get; private set; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort AttackRange { get; private set; }

        public BasicWeapon(uint id, ushort damage, ushort aRange)
            : this(damage, aRange) => Id = id;

        public BasicWeapon(ushort damage, ushort aRange)
        {
            Damage = damage;
            AttackRange = aRange;
        }
    }
}
