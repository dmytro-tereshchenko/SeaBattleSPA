using System.Collections.Generic;
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
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required] 
        public string Name { get; set; }

        [JsonIgnore]
        public ICollection<Game> Games { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GameState()
        {
            Games = new List<Game>();
        }
    }
}
