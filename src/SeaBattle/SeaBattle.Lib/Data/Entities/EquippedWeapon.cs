using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Equipped to <see cref="GameShip"/> weapons
    /// </summary>
    public class EquippedWeapon : IEquippedWeapon
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int WeaponId { get; set;}

        public Weapon Weapon { get; set;}

        public int GameShipId { get; set; }

        public GameShip GameShip { get; set; }
    }
}
