﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SeaBattle.Lib.Entities
{
    public interface ICommonShip
    {
        uint Id { get; }
        ushort AttackRange { get; }
        ushort RepairRange { get; } 
        ushort Damage { get; }

        //amount of hp that ship can repair
        ushort RepairPower { get; }
        ShipType Type { get; }

        //length of the ship (cells), width = 1 cell
        byte Size { get; }

        //Max speed (amount of cells, that the ship can move in 1 turn)
        ushort MaxHp { get; }

        //Max speed
        byte Speed { get; }

        //amount of slots to equip equipment
        byte EquipmentSlots { get; }

    }
}
