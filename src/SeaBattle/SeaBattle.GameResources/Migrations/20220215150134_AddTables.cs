using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class AddTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Repairs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    RepairPower = table.Column<int>(type: "int", nullable: false),
                    RepairRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Repairs", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Weapons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Damage = table.Column<int>(type: "int", nullable: false),
                    AttackRange = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Weapons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    CurrentGamePlayerMove = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxNumberOfPlayers = table.Column<byte>(type: "tinyint", nullable: false),
                    GameStateId = table.Column<long>(type: "bigint", nullable: false),
                    Winner = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_GameStates_GameStateId",
                        column: x => x.GameStateId,
                        principalTable: "GameStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayers",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    PlayerStateId = table.Column<long>(type: "bigint", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                        column: x => x.PlayerStateId,
                        principalTable: "PlayerStates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ships",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    ShipTypeId = table.Column<long>(type: "bigint", nullable: false),
                    Size = table.Column<byte>(type: "tinyint", nullable: false),
                    MaxHp = table.Column<int>(type: "int", nullable: false),
                    Speed = table.Column<byte>(type: "tinyint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ships_ShipTypes_ShipTypeId",
                        column: x => x.ShipTypeId,
                        principalTable: "ShipTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    SizeX = table.Column<int>(type: "int", nullable: false),
                    SizeY = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameFields_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GamePlayerGame",
                columns: table => new
                {
                    GamePlayersId = table.Column<long>(type: "bigint", nullable: false),
                    GamesId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GamePlayerGame", x => new { x.GamePlayersId, x.GamesId });
                    table.ForeignKey(
                        name: "FK_GamePlayerGame_GamePlayers_GamePlayersId",
                        column: x => x.GamePlayersId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GamePlayerGame_Games_GamesId",
                        column: x => x.GamesId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartFields",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    GameId = table.Column<long>(type: "bigint", nullable: false),
                    GameFieldId = table.Column<long>(type: "bigint", nullable: false),
                    GamePlayerId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartFields", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartFields_GameFields_GameFieldId",
                        column: x => x.GameFieldId,
                        principalTable: "GameFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartFields_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StartFields_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameShips",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    Hp = table.Column<int>(type: "int", nullable: false),
                    Points = table.Column<int>(type: "int", nullable: false),
                    ShipId = table.Column<long>(type: "bigint", nullable: false),
                    GamePlayerId = table.Column<long>(type: "bigint", nullable: false),
                    StartFieldId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameShips", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameShips_GamePlayers_GamePlayerId",
                        column: x => x.GamePlayerId,
                        principalTable: "GamePlayers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameShips_Ships_ShipId",
                        column: x => x.ShipId,
                        principalTable: "Ships",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameShips_StartFields_StartFieldId",
                        column: x => x.StartFieldId,
                        principalTable: "StartFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameFieldCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    Stern = table.Column<bool>(type: "bit", nullable: false),
                    GameShipId = table.Column<long>(type: "bigint", nullable: false),
                    GameFieldId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameFieldCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameFieldCells_GameFields_GameFieldId",
                        column: x => x.GameFieldId,
                        principalTable: "GameFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_GameFieldCells_GameShips_GameShipId",
                        column: x => x.GameShipId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "GameShipRepair",
                columns: table => new
                {
                    GameShipsId = table.Column<long>(type: "bigint", nullable: false),
                    RepairsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameShipRepair", x => new { x.GameShipsId, x.RepairsId });
                    table.ForeignKey(
                        name: "FK_GameShipRepair_GameShips_GameShipsId",
                        column: x => x.GameShipsId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameShipRepair_Repairs_RepairsId",
                        column: x => x.RepairsId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GameShipWeapon",
                columns: table => new
                {
                    GameShipsId = table.Column<long>(type: "bigint", nullable: false),
                    WeaponsId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameShipWeapon", x => new { x.GameShipsId, x.WeaponsId });
                    table.ForeignKey(
                        name: "FK_GameShipWeapon_GameShips_GameShipsId",
                        column: x => x.GameShipsId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameShipWeapon_Weapons_WeaponsId",
                        column: x => x.WeaponsId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StartFieldCells",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false),
                    X = table.Column<int>(type: "int", nullable: false),
                    Y = table.Column<int>(type: "int", nullable: false),
                    StartFieldId = table.Column<long>(type: "bigint", nullable: false),
                    GameShipId = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartFieldCells", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StartFieldCells_GameShips_GameShipId",
                        column: x => x.GameShipId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StartFieldCells_StartFields_StartFieldId",
                        column: x => x.StartFieldId,
                        principalTable: "StartFields",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameFieldCells_GameFieldId",
                table: "GameFieldCells",
                column: "GameFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFieldCells_GameShipId",
                table: "GameFieldCells",
                column: "GameShipId");

            migrationBuilder.CreateIndex(
                name: "IX_GameFields_GameId",
                table: "GameFields",
                column: "GameId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayerGame_GamesId",
                table: "GamePlayerGame",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_Name",
                table: "GamePlayers",
                column: "Name");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerStateId",
                table: "GamePlayers",
                column: "PlayerStateId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameStateId",
                table: "Games",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShipRepair_RepairsId",
                table: "GameShipRepair",
                column: "RepairsId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShips_GamePlayerId",
                table: "GameShips",
                column: "GamePlayerId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShips_ShipId",
                table: "GameShips",
                column: "ShipId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShips_StartFieldId",
                table: "GameShips",
                column: "StartFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShipWeapon_WeaponsId",
                table: "GameShipWeapon",
                column: "WeaponsId");

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFieldCells_GameShipId",
                table: "StartFieldCells",
                column: "GameShipId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFieldCells_StartFieldId",
                table: "StartFieldCells",
                column: "StartFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFields_GameFieldId",
                table: "StartFields",
                column: "GameFieldId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFields_GameId",
                table: "StartFields",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFields_GamePlayerId",
                table: "StartFields",
                column: "GamePlayerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameFieldCells");

            migrationBuilder.DropTable(
                name: "GamePlayerGame");

            migrationBuilder.DropTable(
                name: "GameShipRepair");

            migrationBuilder.DropTable(
                name: "GameShipWeapon");

            migrationBuilder.DropTable(
                name: "StartFieldCells");

            migrationBuilder.DropTable(
                name: "Repairs");

            migrationBuilder.DropTable(
                name: "Weapons");

            migrationBuilder.DropTable(
                name: "GameShips");

            migrationBuilder.DropTable(
                name: "Ships");

            migrationBuilder.DropTable(
                name: "StartFields");

            migrationBuilder.DropTable(
                name: "ShipTypes");

            migrationBuilder.DropTable(
                name: "GameFields");

            migrationBuilder.DropTable(
                name: "GamePlayers");

            migrationBuilder.DropTable(
                name: "Games");

            migrationBuilder.DropTable(
                name: "PlayerStates");

            migrationBuilder.DropTable(
                name: "GameStates");
        }
    }
}
