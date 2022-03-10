import { Weapon } from './weapon';
import { Repair } from './repair';

export interface IGameShip {
    id: number
    hp: number
    shipType: number
    size: number
    maxHp: number
    speed: number
    gamePlayerId: number
    weapons: Weapon[]
    repairs: Repair[]
    attackRange: () => number
    repairRange: () => number
    damage: () => number
    repairPower: () => number
}

export class GameShip implements IGameShip {
    id: number;
    hp: number;
    shipType: number;
    size: number;
    maxHp: number;
    speed: number;
    gamePlayerId: number;
    weapons: Weapon[];
    repairs: Repair[];

    constructor() { }

    attackRange(): number {
        return this.weapons.reduce((acc, shot) => acc = acc > shot.attackRange ? acc : shot.attackRange, 0);
    }

    repairRange(): number {
        return this.repairs.reduce((acc, shot) => acc = acc > shot.repairRange ? acc : shot.repairRange, 0);
    }

    damage(): number {
        return this.weapons.reduce((acc, shot) => acc = acc + shot.damage, 0);
    }

    repairPower(): number {
        return this.repairs.reduce((acc, shot) => acc = acc + shot.repairPower, 0);
    }
}