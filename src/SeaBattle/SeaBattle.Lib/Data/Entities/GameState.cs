using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        public uint GameId { get; set; }

        [JsonIgnore]
        [ForeignKey("GameId")]
        public IGame Game { get; set; }
    }
}
