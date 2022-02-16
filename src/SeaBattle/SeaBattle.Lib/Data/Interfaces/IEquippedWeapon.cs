namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Equipped to <see cref="GameShip"/> weapons
    /// </summary>
    public interface IEquippedWeapon : IEntity
    {
        int WeaponId { get; set; }

        Weapon Weapon { get; set; }

        int GameShipId { get; set; }

        GameShip GameShip { get; set; }
    }
}
