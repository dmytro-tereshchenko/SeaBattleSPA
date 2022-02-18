using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Game (match) between players.
    /// </summary>
    public class Game : IGame
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? CurrentGamePlayerMoveId { get; set; }

        [NotMapped]
        public byte CurrentCountPlayers
        {
            get => (byte)GamePlayers.Count;
        }

        [Required]
        public byte MaxNumberOfPlayers { get; set; }

        public int? WinnerId { get; set; }

        public GameField GameField { get; set; }

        [Column(TypeName = "smallint")]
        public GameState GameState { get; set; }

        public ICollection<StartField> StartFields { get; set; }

        public ICollection<GamePlayer> GamePlayers { get; set; }

        public ICollection<SearchGame> SearchGames { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        public Game()
        {
            GamePlayers = new List<GamePlayer>();
            StartFields = new List<StartField>();
            SearchGames = new List<SearchGame>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        /// <param name="id">Id of game</param>
        public Game(int id) : this() => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="Game"/> class
        /// </summary>
        /// <param name="maxNumberOfPlayers">Max amount of players</param>
        public Game(byte maxNumberOfPlayers) : this() => MaxNumberOfPlayers = maxNumberOfPlayers;
    }
}
