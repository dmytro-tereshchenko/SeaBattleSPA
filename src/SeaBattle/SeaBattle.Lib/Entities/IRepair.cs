namespace SeaBattle.Lib.Entities
{
    public interface IRepair : IEntity
    {
        ushort RepairPower { get; }
        
        ushort RepairRange { get; }
    }
}
