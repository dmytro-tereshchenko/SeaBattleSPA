namespace SeaBattle.Lib.Entities
{
    public interface IShip : ICommonShip
    {
        void AddWeapon(IWeapon weapon);
        
        void AddRepair(IRepair repair);
    }
}