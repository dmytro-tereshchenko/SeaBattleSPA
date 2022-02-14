using Microsoft.EntityFrameworkCore;
using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Data.Entities
{
    public class GameContext : DbContext
    {
        public DbSet<IWeapon> Weapons { get; set; }

        public DbSet<IRepair> Repairs { get; set; }

        public DbSet<ICommonShip> CommonShips { get; set; }

        public DbSet<IGame> Games { get; set; }

        public DbSet<IGameField> GameFields { get; set; }

        public DbSet<IGameFieldCell> GameFieldCells { get; set; }

        public DbSet<IGamePlayer> GamePlayers { get; set; }

        public DbSet<IGameShip> GameShips { get; set; }

        public DbSet<IGameState> GameStates { get; set; }

        public DbSet<IPlayerState> PlayerStates { get; set; }

        public DbSet<IShipType> ShipTypes { get; set; }

        public DbSet<IStartField> StartFields { get; set; }

        public DbSet<IStartFieldCell> StartFieldCells { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
    public enum ShipType
    {
        Military,
        Auxiliary,
        Mixed
    }
    public enum PlayerState
    {
        Created, //created (initial)
        InitializeField, //generate start team of ship, initialize field
        Ready, //wait for another player
        Process //in process of game
    }
    public enum GameState
    {
        Created, //Game was created
        SearchPlayers, //Waiting for searching and connecting players, after connect amount of maxPlayers next state Init
        Init, //Initializing game
        Process, //Game in process
        Finished //Game was finished
    }
}
