using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public class GameShip : IGameShip
    {
        /// <summary>
        /// Id Entity
        /// </summary>
        /// <value><see cref="uint"/></value>
        public uint Id { get; set; }

        /// <summary>
        /// Basic ship
        /// </summary>
        /// <value><see cref="ICommonShip"/></value>
        public ICommonShip Ship { get; private set; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort Hp { get; set; }

        /// <summary>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="IPlayer"/></value>
        public IPlayer Player { get; private set; }

        /// <summary>
        /// Number of ship cost points
        /// </summary>
        /// <value><see cref="int"/></value>
        public int Points { get; private set; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort AttackRange { get => Ship.AttackRange; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        public ushort RepairRange { get => Ship.RepairRange; }

        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
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

        /// <summary>
        /// Max hp of the ship that he can be damaged
        /// </summary>
        /// <value><see cref="byte"/></value>
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

        public GameShip(uint id, ICommonShip ship, IPlayer player, int points, ushort hp)
            : this(ship, player, points, hp) => Id = id;

        public GameShip(uint id, ICommonShip ship, IPlayer player, int points) 
            : this(id, ship, player, points, ship.MaxHp) { }

        public GameShip(ICommonShip ship, IPlayer player, int points, ushort hp)
        {
            Ship = ship;
            Player = player;
            Points = points;
            Hp = hp;
        }

        public GameShip(ICommonShip ship, IPlayer player, int points)
            : this(ship, player, points, ship.MaxHp) { }
    }
}
