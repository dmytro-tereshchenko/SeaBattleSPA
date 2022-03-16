using Microsoft.EntityFrameworkCore;
using SeaBattle.Lib.Entities;

namespace SeaBattle.Lib.Data.Entities
{
    public class GameDbContext : DbContext
    {
        public DbSet<Weapon> Weapons { get; set; }

        public DbSet<Repair> Repairs { get; set; }

        public DbSet<Ship> Ships { get; set; }

        public DbSet<Game> Games { get; set; }

        public DbSet<GameField> GameFields { get; set; }

        public DbSet<GameFieldCell> GameFieldCells { get; set; }

        public DbSet<GamePlayer> GamePlayers { get; set; }

        public DbSet<GameShip> GameShips { get; set; }

        public DbSet<StartField> StartFields { get; set; }

        public DbSet<StartFieldCell> StartFieldCells { get; set; }

        public GameDbContext(DbContextOptions<GameDbContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Add special foreign key
            modelBuilder
                .Entity<GameShip>()
                .HasMany(c => c.Weapons)
                .WithMany(s => s.GameShips)
                .UsingEntity<EquippedWeapon>(
                    j => j
                        .HasOne(pt => pt.Weapon)
                        .WithMany(t => t.EquippedWeapons)
                        .HasForeignKey(pt => pt.WeaponId)
                        .OnDelete(DeleteBehavior.Restrict),
                    j => j
                        .HasOne(pt => pt.GameShip)
                        .WithMany(p => p.EquippedWeapons)
                        .HasForeignKey(pt => pt.GameShipId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                        j.HasKey(t => new { t.Id, t.GameShipId, t.WeaponId });
                        j.ToTable("EquippedWeapons");
                    });

            modelBuilder
                .Entity<GameShip>()
                .HasMany(c => c.Repairs)
                .WithMany(s => s.GameShips)
                .UsingEntity<EquippedRepair>(
                    j => j
                        .HasOne(pt => pt.Repair)
                        .WithMany(t => t.EquippedRepairs)
                        .HasForeignKey(pt => pt.RepairId)
                        .OnDelete(DeleteBehavior.Restrict)
                        ,
                    j => j
                        .HasOne(pt => pt.GameShip)
                        .WithMany(p => p.EquippedRepairs)
                        .HasForeignKey(pt => pt.GameShipId)
                        .OnDelete(DeleteBehavior.Cascade),
                    j =>
                    {
                        j.Property(pt => pt.Id).ValueGeneratedOnAdd();
                        j.HasKey(t => new { t.Id, t.GameShipId, t.RepairId });
                        j.ToTable("EquippedRepairs");
                    });

            modelBuilder.Entity<Game>()
                .HasMany(g => g.GamePlayers)
                .WithMany(p => p.Games)
                .UsingEntity(j => j.ToTable("GamePlayerGame"));

            modelBuilder.Entity<Game>()
                .HasMany(g => g.StartFields)
                .WithOne(s => s.Game)
                .HasForeignKey(s => s.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StartField>()
                .HasMany(f => f.GameShips)
                .WithOne(s => s.StartField)
                .HasForeignKey(s => s.StartFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StartField>()
                .HasMany(f => f.StartFieldCells)
                .WithOne(s => s.StartField)
                .HasForeignKey(s => s.StartFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GameShip>()
                .HasMany(s => s.GameFieldCells)
                .WithOne(c => c.GameShip)
                .HasForeignKey(c => c.GameShipId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<GameField>()
                .HasMany(f => f.GameFieldCells)
                .WithOne(c => c.GameField)
                .HasForeignKey(c => c.GameFieldId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Ship>()
                .HasMany(s => s.GameShips)
                .WithOne(g => g.Ship)
                .HasForeignKey(g => g.ShipId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GamePlayer>()
                .HasMany(p => p.GameShips)
                .WithOne(g => g.GamePlayer)
                .HasForeignKey(g => g.GamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Game>()
                .HasOne(g => g.GameField)
                .WithOne(f => f.Game)
                .HasForeignKey<GameField>(f => f.GameId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GamePlayer>()
                .HasMany(p => p.StartFields)
                .WithOne(f => f.GamePlayer)
                .HasForeignKey(f => f.GamePlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<GamePlayer>().Property(p => p.PlayerState).HasDefaultValue(PlayerState.Created);

            modelBuilder.Entity<Game>().Property(p => p.GameState).HasDefaultValue(GameState.Created);

            modelBuilder.Entity<GameFieldCell>().Property(p => p.Stern).HasDefaultValue(false);

            //initialize data
            modelBuilder.Entity<Weapon>().HasData(new Weapon() { Id = 1, AttackRange = 10, Damage = 50 });

            modelBuilder.Entity<Repair>().HasData(new Repair() { Id = 1, RepairRange = 10, RepairPower = 40 });

            modelBuilder.Entity<Ship>().HasData(
                new Ship[]
                {
                    new Ship {Id = 1, Size = 1, MaxHp = 100, Speed = 4, ShipType = ShipType.Auxiliary, Cost = 1000},
                    new Ship {Id = 2, Size = 2, MaxHp = 200, Speed = 3, ShipType = ShipType.Mixed, Cost = 2000},
                    new Ship {Id = 3, Size = 3, MaxHp = 300, Speed = 2, ShipType = ShipType.Mixed, Cost = 3000},
                    new Ship {Id = 4, Size = 4, MaxHp = 400, Speed = 1, ShipType = ShipType.Military, Cost = 4000}
                });
        }
    }
}
