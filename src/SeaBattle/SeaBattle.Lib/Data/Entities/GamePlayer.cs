using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    [Index("Name")]
    public class GamePlayer : Player, IGamePlayer
    {
        [JsonIgnore]
        public uint PlayerStateId { get; set; }

        [ForeignKey("PlayerStateId")]
        public IPlayerState PlayerState { get; set; }

        [JsonIgnore]
        public ICollection<IGameShip> GameShips { get; set; }

        [JsonIgnore]
        public ICollection<IGame> Games { get; set; }

        [JsonIgnore]
        public ICollection<IStartField> StartFields { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public GamePlayer()
        {
            GameShips = new List<IGameShip>();
            Games = new List<IGame>();
            StartFields = new List<IStartField>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GamePlayer"/> class
        /// </summary>
        /// <param name="id">Id of game player</param>
        /// <param name="name">Players name</param>
        public GamePlayer(uint id, string name) : this(name) => Id = id;

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
