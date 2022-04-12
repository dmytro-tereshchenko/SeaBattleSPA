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

    constructor(payload: Partial<GameShip>) {
        this.id = payload.id || 0
        this.hp = payload.hp || 0
        this.shipType = payload.shipType || 0
        this.size = payload.size || 0
        this.maxHp = payload.maxHp || 0
        this.speed = payload.speed || 0
        this.gamePlayerId = payload.gamePlayerId || 0
        this.weapons = payload.weapons || []
        this.repairs = payload.repairs || []
    };

    attackRange = (): number => this.weapons.reduce((acc, shot) => acc = acc > shot.attackRange ? acc : shot.attackRange, 0);

    repairRange = (): number => this.repairs.reduce((acc, shot) => acc = acc > shot.repairRange ? acc : shot.repairRange, 0);

    damage = (): number => this.weapons.reduce((acc, shot) => acc = acc + shot.damage, 0);

    repairPower = (): number => this.repairs.reduce((acc, shot) => acc = acc + shot.repairPower, 0);
}