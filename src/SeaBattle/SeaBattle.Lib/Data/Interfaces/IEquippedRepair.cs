namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Equipped to <see cref="GameShip"/> repairs
    /// </summary>
    public interface IEquippedRepair : IEntity
    {
        int RepairId { get; set; }

        Repair Repair { get; set; }

        int GameShipId { get; set; }

        GameShip GameShip { get; set; }
    }
}
