using System.Collections.Generic;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// An expanded ship that is used in the game
    /// </summary>
    public interface IGameShip : ICommonShip, IEntity
    {
        /// <summary>
        /// Foreign key Id <see cref="Ship"/>
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint ShipId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="GamePlayer"/>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint GamePlayerId { get; set; }

        /// <summary>
        /// Foreign key Id <see cref="StartField"/>
        /// The field for storing the location of ships and points for buy ships by the player when initializing game.
        /// </summary>
        /// <value><see cref="uint"/></value>
        uint? StartFieldId { get; set; }

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
        /// Navigate property <see cref="BasicWeapon"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="BasicWeapon"/></value>
        ICollection<BasicWeapon> Weapons { get; set;  }

        /// <summary>
        /// Collection of weapons on ship
        /// Navigate property <see cref="BasicRepair"/>
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="BasicRepair"/></value>
        ICollection<BasicRepair> Repairs { get; set; }

        /// <summary>
        /// Navigate property <see cref="Ship"/>
        /// </summary>
        /// <value><see cref="Ship"/></value>
        Ship Ship { get; set; }

        /// <summary>
        /// Navigate property <see cref="GamePlayer"/>
        /// The player who owns this ship
        /// </summary>
        /// <value><see cref="GamePlayer"/></value>
        GamePlayer GamePlayer { get; set; }

        /// <summary>
        /// Navigate property <see cref="StartField"/>
        /// The field for storing the location of ships and points for buy ships by the player when initializing game.
        /// </summary>
        /// <value><see cref="StartField"/></value>
        StartField StartField { get; set; }

        /// <summary>
        /// Collection of field's cell for allocate <see cref="GameShip"/> on <see cref="GameField"/> 
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="GameFieldCell"/></value>
        ICollection<GameFieldCell> GameFieldCells { get; set; }

        /// <summary>
        /// Collection of field's cell for storing the location of ships and points for buy ships by the player when initializing game.
        /// </summary>
        /// <value><see cref="ICollection{T}"/> whose generic type argument is <see cref="StartFieldCell"/></value>
        ICollection<StartFieldCell> StartFieldCells { get; set; }
    }
}
