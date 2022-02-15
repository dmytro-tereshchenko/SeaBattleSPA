using Microsoft.EntityFrameworkCore;
using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Data.Entities
{
    public class GameContext : DbContext
    {
        public DbSet<IWeapon> Weapons { get; set; }

        public DbSet<IRepair> Repairs { get; set; }

        public DbSet<IShip> Ships { get; set; }

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

            modelBuilder.Entity<IGameShip>()
                .HasMany(s => s.Weapons)
                .WithMany(w => w.GameShips)
                .UsingEntity(j => j.ToTable("GameShipWeapon"));

            modelBuilder.Entity<IGameShip>()
                .HasMany(s => s.Repairs)
                .WithMany(r => r.GameShips)
                .UsingEntity(j => j.ToTable("GameShipRepair"));

            modelBuilder.Entity<IGame>()
                .HasMany(g => g.GamePlayers)
                .WithMany(p => p.Games)
                .UsingEntity(j => j.ToTable("GamePlayerGame"));
        }
    }
    /*public enum ShipType
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
    }*/
}
