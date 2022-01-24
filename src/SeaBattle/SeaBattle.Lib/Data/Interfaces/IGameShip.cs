namespace SeaBattle.Lib.Entities
{
    public interface IGameShip : ICommonShip
    { 
        IShip Ship { get; }

        //current hp
        ushort Hp { get; set; }

        uint PlayerId { get; }

        int Points { get; }
    }
}
