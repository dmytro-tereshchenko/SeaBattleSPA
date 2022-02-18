using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    [Index("Name")]
    public class GamePlayer : Player, IGamePlayer
    {
        [Required]
        [Column(TypeName= "smallint")]
        public PlayerState PlayerState { get; set; }

        public ICollection<GameShip> GameShips { get; set; }

        public ICollection<Game> Games { get; set; }

        public ICollection<StartField> StartFields { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayer()
        {
            GameShips = new List<GameShip>();
            Games = new List<Game>();
            StartFields = new List<StartField>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayer"/> class
        /// </summary>
        /// <param name="id">Id of game player</param>
        /// <param name="name">Players name</param>
        public GamePlayer(int id, string name) : this(name) => Id = id;

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayer"/> class
        /// </summary>
        /// <param name="name">Players name</param>
        public GamePlayer(string name) : this() => Name = name;

        public static bool operator ==(GamePlayer obj1, GamePlayer obj2) =>
            obj1?.Equals(obj2) ?? false;

        public static bool operator !=(GamePlayer obj1, GamePlayer obj2) =>
            !(obj1==obj2);

        public override bool Equals(object? obj)
        {
            if (obj is null || obj is not GamePlayer)
            {
                return false;
            }

            GamePlayer gamePlayer = (obj as GamePlayer)!;

            return gamePlayer?.Name == this.Name && gamePlayer.Id == this.Id;
        }

        public override int GetHashCode()
        {
            return (Name + Id).GetHashCode() + base.GetHashCode();
        }
    }
}
