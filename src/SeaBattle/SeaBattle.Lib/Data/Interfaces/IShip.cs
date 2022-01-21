namespace SeaBattle.Lib.Entities
{
    public interface IShip : ICommonShip
    {
        void AddWeapon(IWeapon weapon);
        
        void AddRepair(IRepair repair);

        bool RemoveWeapon(IWeapon weapon);

        bool RemoveRepair(IRepair repair);
    }
}
