namespace SeaBattle.Lib.Entities
{
    public class BasicWeapon : IWeapon
    {
        private uint _id;

        private ushort _damage;

        private ushort _attackRange;

        public uint Id
        {
            get => _id;
            private set => _id = value;
        }

        public ushort Damage { get => _damage; }

        public ushort AttackRange { get => _attackRange; }

        public BasicWeapon(uint id, ushort damage, ushort aRange)
            : this(damage, aRange)
        {
            _id = id;
        }

        public BasicWeapon(ushort damage, ushort aRange)
        {
            _damage = damage;
            _attackRange = aRange;
        }
    }
}
