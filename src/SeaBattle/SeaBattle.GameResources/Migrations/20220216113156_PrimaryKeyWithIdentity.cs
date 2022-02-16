using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class PrimaryKeyWithIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipNewId",
                table: "StartFieldCells");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Weapons",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "StartFields",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "GameShipNewId",
                table: "StartFieldCells",
                newName: "GameShipId");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "StartFieldCells",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_StartFieldCells_GameShipNewId",
                table: "StartFieldCells",
                newName: "IX_StartFieldCells_GameShipId");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "ShipTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Ships",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Repairs",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "PlayerStates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GameStates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GameShips",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "Games",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GamePlayers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GameFields",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GameFieldCells",
                newName: "Id");

            migrationBuilder.CreateTable(
                name: "GamePlayerGame",
                columns: table => new
                {
                    GamePlayersId = table.Column<int>(type: "int", nullable: false),
                    GamesId = table.Column<int>(type: "int", nullable: false)
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
                name: "GameShipRepair",
                columns: table => new
                {
                    GameShipsId = table.Column<int>(type: "int", nullable: false),
                    RepairsId = table.Column<int>(type: "int", nullable: false)
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
                    GameShipsId = table.Column<int>(type: "int", nullable: false),
                    WeaponsId = table.Column<int>(type: "int", nullable: false)
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

            migrationBuilder.InsertData(
                table: "GameStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "SearchPlayers" },
                    { 3, "Init" },
                    { 4, "Process" },
                    { 5, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "PlayerStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Created" },
                    { 2, "InitializeField" },
                    { 3, "Ready" },
                    { 4, "Process" }
                });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "RepairPower", "RepairRange" },
                values: new object[] { 1, 40, 10 });

            migrationBuilder.InsertData(
                table: "ShipTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Military" },
                    { 2, "Auxiliary" },
                    { 3, "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AttackRange", "Damage" },
                values: new object[] { 1, 10, 50 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "Id", "Cost", "MaxHp", "ShipTypeId", "Size", "Speed" },
                values: new object[,]
                {
                    { 4, 4000L, 400, 1, (byte)4, (byte)1 },
                    { 2, 2000L, 200, 2, (byte)2, (byte)3 },
                    { 3, 3000L, 300, 2, (byte)3, (byte)2 },
                    { 1, 1000L, 100, 3, (byte)1, (byte)4 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayerGame_GamesId",
                table: "GamePlayerGame",
                column: "GamesId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShipRepair_RepairsId",
                table: "GameShipRepair",
                column: "RepairsId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShipWeapon_WeaponsId",
                table: "GameShipWeapon",
                column: "WeaponsId");

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipId",
                table: "StartFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipId",
                table: "StartFieldCells");

            migrationBuilder.DropTable(
                name: "GamePlayerGame");

            migrationBuilder.DropTable(
                name: "GameShipRepair");

            migrationBuilder.DropTable(
                name: "GameShipWeapon");

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Repairs",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Weapons",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StartFields",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "GameShipId",
                table: "StartFieldCells",
                newName: "GameShipNewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "StartFieldCells",
                newName: "NewId");

            migrationBuilder.RenameIndex(
                name: "IX_StartFieldCells_GameShipId",
                table: "StartFieldCells",
                newName: "IX_StartFieldCells_GameShipNewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ShipTypes",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Ships",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Repairs",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PlayerStates",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameStates",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameShips",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Games",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GamePlayers",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameFields",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameFieldCells",
                newName: "NewId");

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipNewId",
                table: "StartFieldCells",
                column: "GameShipNewId",
                principalTable: "GameShips",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
