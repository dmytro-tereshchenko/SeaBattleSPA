namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic weapon equipment for ship
    /// </summary>
    public class BasicWeapon : IWeapon
    {
        public uint Id { get; set; }

        public ushort Damage { get; private set; }

        public ushort AttackRange { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicWeapon"/> class
        /// </summary>
        /// <param name="id">Id of weapon</param>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public BasicWeapon(uint id, ushort damage, ushort aRange)
            : this(damage, aRange) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="BasicWeapon"/> class
        /// </summary>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public BasicWeapon(ushort damage, ushort aRange)
        {
            Damage = damage;
            AttackRange = aRange;
        }
    }
}
