﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Basic weapon equipment for ship
    /// </summary>
    public class Weapon : IWeapon
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public ushort Damage { get; set; }

        [Required]
        public ushort AttackRange { get; set; }

        public ICollection<GameShip> GameShips { get; set; }

        public ICollection<EquippedWeapon> EquippedWeapons { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public Weapon()
        {
            GameShips = new List<GameShip>();
            EquippedWeapons = new List<EquippedWeapon>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class
        /// </summary>
        /// <param name="id">Id of weapon</param>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public Weapon(int id, ushort damage, ushort aRange)
            : this(damage, aRange) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Weapon"/> class
        /// </summary>
        /// <param name="damage">Amount of hp that target ship can be damaged</param>
        /// <param name="aRange">Distance to target ship which can be damaged</param>
        public Weapon(ushort damage, ushort aRange) : this()
        {
            Damage = damage;
            AttackRange = aRange;
        }
    }
}
