namespace SeaBattle.Lib.Entities
{
    public interface IGameShip : ICommonShip
    { 
        ICommonShip Ship { get; }

        /// <summary>
        /// Current hp of ship
        /// </summary>
        /// <value><see cref="ushort"/></value>
        ushort Hp { get; set; }

        uint PlayerId { get; }

        int Points { get; }
    }
}
