﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface IWeapon : IEntity
    {
        
        ushort Damage { get; }

        ushort AttackRange { get; }

    }
}
