using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    public class GameShip : IGameShip
    {
        public uint Id { get; set; }

        public ICommonShip Ship { get; private set; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort Hp { get; set; }

        public uint PlayerId { get; private set; }

        public int Points { get; private set; }

        public ushort AttackRange { get => Ship.AttackRange; }
        
        public ushort RepairRange { get => Ship.RepairRange; }
        
        public ushort Damage { get => Ship.Damage; }

        /// <summary>
        /// Amount of hp that ship can repair
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairPower { get => Ship.RepairPower; }
        
        public ShipType Type { get => Ship.Type; }

        /// <summary>
        /// Length of the ship (cells) and amount of possible equipment slots, width = 1 cell
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte Size { get => Ship.Size; }
        
        public ushort MaxHp { get => Ship.MaxHp; }

        /// <summary>
        /// Max speed (amount of cells, that the ship can move in 1 turn)
        /// </summary>
        /// <value><see cref="byte"/></value>
        public byte Speed { get => Ship.Speed; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        public ICollection<IWeapon> Weapons { get=>Ship.Weapons; set=>Ship.Weapons=value; }

        /// <summary>
        /// Collection of weapons on ship
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        public ICollection<IRepair> Repairs { get => Ship.Repairs; set => Ship.Repairs = value; }

        public GameShip(uint id, ICommonShip ship, uint teamId, int points, ushort hp)
            : this(ship, teamId, points, hp) => Id = id;

        public GameShip(uint id, ICommonShip ship, uint teamId, int points) 
            : this(id, ship, teamId, points, ship.MaxHp) { }

        public GameShip(ICommonShip ship, uint playerId, int points, ushort hp)
        {
            Ship = ship;
            PlayerId = playerId;
            Points = points;
            Hp = hp;
        }

        public GameShip(ICommonShip ship, uint playerId, int points)
            : this(ship, playerId, points, ship.MaxHp) { }
    }
}
