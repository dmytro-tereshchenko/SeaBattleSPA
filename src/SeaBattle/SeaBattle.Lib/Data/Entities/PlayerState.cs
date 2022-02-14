using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        public ICollection<IGamePlayer> GamePlayers { get; set; } = new List<IGamePlayer>();
    }
}
