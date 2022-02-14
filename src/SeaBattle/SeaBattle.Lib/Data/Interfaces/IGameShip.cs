using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public interface IGameShip : IEntity
    {
        /// <summary>
        /// Foreign key Id <see cref="ICommonShip"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint CommonShipId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="IGamePlayer"/>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GamePlayerId { get; set; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Hp { get; set; }

        /// <summary>
        /// Number of ship cost points
        /// </summary>
        /// <value><see cref="int"/></value>
        int Points { get; set; }

        /// <summary>
        /// Distance to target ship which can be damaged.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort AttackRange { get; }

        /// <summary>
        /// Distance to target ship which can be repaired.
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairRange { get; }

        /// <summary>
        /// Amount of hp that target ship can be damaged
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Damage { get; }

        /// <summary>
        /// Amount of hp that ship can be repaired
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort RepairPower { get; }

        /// <summary>
        /// Collection of weapons on ship
        /// Navigate property <see cref="IWeapon/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IWeapon"/></value>
        ICollection<IWeapon> Weapons { get; set;  }

        /// <summary>
        /// Collection of weapons on ship
        /// Navigate property <see cref="IRepair/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IRepair"/></value>
        ICollection<IRepair> Repairs { get; set; }

        /// <summary>
        /// Navigate property <see cref="ICommonShip"/>
        /// </summary>
        /// <value><see cref="ICommonShip"/></value>
        ICommonShip CommonShip { get; set; }

        /// <summary>
        /// Navigate property <see cref="IGamePlayer/>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="IGamePlayer"/></value>
        IGamePlayer GamePlayer { get; set; }

        /// <summary>
        /// Collection of field's cell for allocate <see cref="IGameShip"/> on <see cref="IGameField"/> 
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IGameFieldCell"/></value>
        ICollection<IGameFieldCell> GameFieldCells { get; set; }

        /// <summary>
        /// Collection of field's cell for storing the location of ships and points for buy ships by the player when initializing game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="IStartFieldCell"/></value>
        ICollection<IStartFieldCell> StartFieldCells { get; set; }
    }
}
