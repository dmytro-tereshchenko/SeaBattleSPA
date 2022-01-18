using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public class BasicWeapon : IWeapon, IEntity
    {
        private uint _id;

        private ushort _damage;

        private ushort _attackRange;

        public uint Id { get => _id; }
        public ushort Damage { get => _damage; }
        public ushort AttackRange { get => _attackRange; }
        public BasicWeapon(uint id, ushort damage, ushort aRange)
        {
            this._id = id;
            this._damage = damage;
            this._attackRange = aRange;
        }
    }
}
