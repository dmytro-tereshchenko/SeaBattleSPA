﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player's state in game
    /// </summary>
    public class PlayerState : IPlayerState
    {
        [Key]
        public uint Id { get; set; }

        [Required]
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<GamePlayer> GamePlayers { get; set; } = new List<GamePlayer>();

        /// <summary>
        /// Default constructor
        /// </summary>
        public PlayerState(){}
    }
}
