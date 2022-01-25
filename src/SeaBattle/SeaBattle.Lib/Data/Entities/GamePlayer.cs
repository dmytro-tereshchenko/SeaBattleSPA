using SeaBattle.Lib.Data;
using SeaBattle.Lib.Infrastructure;

namespace SeaBattle.Lib.Entities
{
    /// <summary>
    /// Player (user) in the game
    /// </summary>
    public class GamePlayer : Player, IGamePlayer
    {
        public PlayerState State { get; set; }

        public GamePlayer(uint id, string name) : this(name) => Id = id;

        public GamePlayer(string name) : base(name)
        {
            Name = name;
            State = PlayerState.Created;
        }

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
