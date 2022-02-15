using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Possible states of the game.
    /// </summary>
    public class GameState : IGameState
    {
        [Key] 
        public uint Id { get; set; }

        [Required] 
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<IGame> Games { get; set; } = new List<IGame>();
    }
}
