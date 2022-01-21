namespace SeaBattle.Lib.Entities
{
    public class BasicWeapon : IWeapon
    {
        public uint Id { get; set; }

        public ushort Damage { get; private set; }

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
