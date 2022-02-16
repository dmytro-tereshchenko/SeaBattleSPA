﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SeaBattle.Lib.Data.Entities;

namespace SeaBattle.GameResources.Migrations
{
    [DbContext(typeof(GameDbContext))]
    [Migration("20220216131206_UpdateStringTipes")]
    partial class UpdateStringTipes
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.14")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GameGamePlayer", b =>
                {
                    b.Property<int>("GamePlayersId")
                        .HasColumnType("int");

                    b.Property<int>("GamesId")
                        .HasColumnType("int");

                    b.HasKey("GamePlayersId", "GamesId");

                    b.HasIndex("GamesId");

                    b.ToTable("GamePlayerGame");
                });

            modelBuilder.Entity("GameShipRepair", b =>
                {
                    b.Property<int>("GameShipsId")
                        .HasColumnType("int");

                    b.Property<int>("RepairsId")
                        .HasColumnType("int");

                    b.HasKey("GameShipsId", "RepairsId");

                    b.HasIndex("RepairsId");

                    b.ToTable("GameShipRepair");
                });

            modelBuilder.Entity("GameShipWeapon", b =>
                {
                    b.Property<int>("GameShipsId")
                        .HasColumnType("int");

                    b.Property<int>("WeaponsId")
                        .HasColumnType("int");

                    b.HasKey("GameShipsId", "WeaponsId");

                    b.HasIndex("WeaponsId");

                    b.ToTable("GameShipWeapon");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CurrentGamePlayerMove")
                        .HasColumnType("int");

                    b.Property<short>("GameStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)1);

                    b.Property<byte>("MaxNumberOfPlayers")
                        .HasColumnType("tinyint");

                    b.Property<int?>("Winner")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameStateId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("SizeX")
                        .HasColumnType("int");

                    b.Property<int>("SizeY")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("GameFields");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameFieldCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameFieldId")
                        .HasColumnType("int");

                    b.Property<int>("GameShipId")
                        .HasColumnType("int");

                    b.Property<bool>("Stern")
                        .HasColumnType("bit");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameFieldId");

                    b.HasIndex("GameShipId");

                    b.ToTable("GameFieldCells");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GamePlayer", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<short>("PlayerStateId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasDefaultValue((short)1);

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.HasIndex("PlayerStateId");

                    b.ToTable("GamePlayers");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameShip", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GamePlayerId")
                        .HasColumnType("int");

                    b.Property<int>("Hp")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<int>("ShipId")
                        .HasColumnType("int");

                    b.Property<int?>("StartFieldId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GamePlayerId");

                    b.HasIndex("ShipId");

                    b.HasIndex("StartFieldId");

                    b.ToTable("GameShips");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameState", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("GameStates");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Created"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "SearchPlayers"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Init"
                        },
                        new
                        {
                            Id = (short)4,
                            Name = "Process"
                        },
                        new
                        {
                            Id = (short)5,
                            Name = "Finished"
                        });
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.PlayerState", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("PlayerStates");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Created"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "InitializeField"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Ready"
                        },
                        new
                        {
                            Id = (short)4,
                            Name = "Process"
                        });
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Repair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RepairPower")
                        .HasColumnType("int");

                    b.Property<int>("RepairRange")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Repairs");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            RepairPower = 40,
                            RepairRange = 10
                        });
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Ship", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<long>("Cost")
                        .HasColumnType("bigint");

                    b.Property<int>("MaxHp")
                        .HasColumnType("int");

                    b.Property<short>("ShipTypeId")
                        .HasColumnType("smallint");

                    b.Property<byte>("Size")
                        .HasColumnType("tinyint");

                    b.Property<byte>("Speed")
                        .HasColumnType("tinyint");

                    b.HasKey("Id");

                    b.HasIndex("ShipTypeId");

                    b.ToTable("Ships");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Cost = 1000L,
                            MaxHp = 100,
                            ShipTypeId = (short)3,
                            Size = (byte)1,
                            Speed = (byte)4
                        },
                        new
                        {
                            Id = 2,
                            Cost = 2000L,
                            MaxHp = 200,
                            ShipTypeId = (short)2,
                            Size = (byte)2,
                            Speed = (byte)3
                        },
                        new
                        {
                            Id = 3,
                            Cost = 3000L,
                            MaxHp = 300,
                            ShipTypeId = (short)2,
                            Size = (byte)3,
                            Speed = (byte)2
                        },
                        new
                        {
                            Id = 4,
                            Cost = 4000L,
                            MaxHp = 400,
                            ShipTypeId = (short)1,
                            Size = (byte)4,
                            Speed = (byte)1
                        });
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.ShipType", b =>
                {
                    b.Property<short>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("smallint")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("ShipTypes");

                    b.HasData(
                        new
                        {
                            Id = (short)1,
                            Name = "Military"
                        },
                        new
                        {
                            Id = (short)2,
                            Name = "Auxiliary"
                        },
                        new
                        {
                            Id = (short)3,
                            Name = "Mixed"
                        });
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.StartField", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("GameFieldId")
                        .HasColumnType("int");

                    b.Property<int>("GameId")
                        .HasColumnType("int");

                    b.Property<int>("GamePlayerId")
                        .HasColumnType("int");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameFieldId");

                    b.HasIndex("GameId");

                    b.HasIndex("GamePlayerId");

                    b.ToTable("StartFields");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.StartFieldCell", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("GameShipId")
                        .HasColumnType("int");

                    b.Property<int>("StartFieldId")
                        .HasColumnType("int");

                    b.Property<int>("X")
                        .HasColumnType("int");

                    b.Property<int>("Y")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("GameShipId");

                    b.HasIndex("StartFieldId");

                    b.ToTable("StartFieldCells");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Weapon", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AttackRange")
                        .HasColumnType("int");

                    b.Property<int>("Damage")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Weapons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AttackRange = 10,
                            Damage = 50
                        });
                });

            modelBuilder.Entity("GameGamePlayer", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GamePlayer", null)
                        .WithMany()
                        .HasForeignKey("GamePlayersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.Game", null)
                        .WithMany()
                        .HasForeignKey("GamesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameShipRepair", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameShip", null)
                        .WithMany()
                        .HasForeignKey("GameShipsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.Repair", null)
                        .WithMany()
                        .HasForeignKey("RepairsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GameShipWeapon", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameShip", null)
                        .WithMany()
                        .HasForeignKey("GameShipsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.Weapon", null)
                        .WithMany()
                        .HasForeignKey("WeaponsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Game", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameState", "GameState")
                        .WithMany("Games")
                        .HasForeignKey("GameStateId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GameState");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameField", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.Game", "Game")
                        .WithOne("GameField")
                        .HasForeignKey("SeaBattle.Lib.Entities.GameField", "GameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameFieldCell", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameField", "GameField")
                        .WithMany("GameFieldCells")
                        .HasForeignKey("GameFieldId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.GameShip", "GameShip")
                        .WithMany("GameFieldCells")
                        .HasForeignKey("GameShipId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("GameField");

                    b.Navigation("GameShip");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GamePlayer", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.PlayerState", "PlayerState")
                        .WithMany("GamePlayers")
                        .HasForeignKey("PlayerStateId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("PlayerState");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameShip", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GamePlayer", "GamePlayer")
                        .WithMany("GameShips")
                        .HasForeignKey("GamePlayerId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.Ship", "Ship")
                        .WithMany("GameShips")
                        .HasForeignKey("ShipId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.StartField", "StartField")
                        .WithMany("GameShips")
                        .HasForeignKey("StartFieldId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.Navigation("GamePlayer");

                    b.Navigation("Ship");

                    b.Navigation("StartField");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Ship", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.ShipType", "ShipType")
                        .WithMany("Ships")
                        .HasForeignKey("ShipTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ShipType");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.StartField", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameField", "GameField")
                        .WithMany("StartFields")
                        .HasForeignKey("GameFieldId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.Game", "Game")
                        .WithMany("StartFields")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("SeaBattle.Lib.Entities.GamePlayer", "GamePlayer")
                        .WithMany("StartFields")
                        .HasForeignKey("GamePlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("GameField");

                    b.Navigation("GamePlayer");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.StartFieldCell", b =>
                {
                    b.HasOne("SeaBattle.Lib.Entities.GameShip", null)
                        .WithMany("StartFieldCells")
                        .HasForeignKey("GameShipId");

                    b.HasOne("SeaBattle.Lib.Entities.StartField", "StartField")
                        .WithMany("StartFieldCells")
                        .HasForeignKey("StartFieldId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("StartField");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Game", b =>
                {
                    b.Navigation("GameField");

                    b.Navigation("StartFields");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameField", b =>
                {
                    b.Navigation("GameFieldCells");

                    b.Navigation("StartFields");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GamePlayer", b =>
                {
                    b.Navigation("GameShips");

                    b.Navigation("StartFields");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameShip", b =>
                {
                    b.Navigation("GameFieldCells");

                    b.Navigation("StartFieldCells");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.GameState", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.PlayerState", b =>
                {
                    b.Navigation("GamePlayers");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.Ship", b =>
                {
                    b.Navigation("GameShips");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.ShipType", b =>
                {
                    b.Navigation("Ships");
                });

            modelBuilder.Entity("SeaBattle.Lib.Entities.StartField", b =>
                {
                    b.Navigation("GameShips");

                    b.Navigation("StartFieldCells");
                });
#pragma warning restore 612, 618
        }
    }
}
