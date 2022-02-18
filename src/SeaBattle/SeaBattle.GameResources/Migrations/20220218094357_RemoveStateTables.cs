using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class RemoveStateTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropTable(
                name: "GameStates");

            migrationBuilder.DropTable(
                name: "PlayerStates");

            migrationBuilder.DropTable(
                name: "ShipTypes");

            migrationBuilder.DropIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameStateId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.RenameColumn(
                name: "ShipTypeId",
                table: "Ships",
                newName: "ShipType");

            migrationBuilder.RenameColumn(
                name: "GameStateId",
                table: "Games",
                newName: "GameState");

            migrationBuilder.RenameColumn(
                name: "PlayerStateId",
                table: "GamePlayers",
                newName: "PlayerState");

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShipType",
                value: (short)2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShipType",
                value: (short)3);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "ShipType",
                value: (short)3);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ShipType",
                table: "Ships",
                newName: "ShipTypeId");

            migrationBuilder.RenameColumn(
                name: "GameState",
                table: "Games",
                newName: "GameStateId");

            migrationBuilder.RenameColumn(
                name: "PlayerState",
                table: "GamePlayers",
                newName: "PlayerStateId");

            migrationBuilder.CreateTable(
                name: "GameStates",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlayerStates",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlayerStates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ShipTypes",
                columns: table => new
                {
                    Id = table.Column<short>(type: "smallint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShipTypes", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "GameStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Created" },
                    { (short)2, "SearchPlayers" },
                    { (short)3, "Init" },
                    { (short)4, "Process" },
                    { (short)5, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "PlayerStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Created" },
                    { (short)2, "InitializeField" },
                    { (short)3, "Ready" },
                    { (short)4, "Process" }
                });

            migrationBuilder.InsertData(
                table: "ShipTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { (short)1, "Military" },
                    { (short)2, "Auxiliary" },
                    { (short)3, "Mixed" }
                });

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShipTypeId",
                value: (short)3);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShipTypeId",
                value: (short)2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "ShipTypeId",
                value: (short)2);

            migrationBuilder.CreateIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_GameStateId",
                table: "Games",
                column: "GameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_PlayerStateId",
                table: "GamePlayers",
                column: "PlayerStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                table: "GamePlayers",
                column: "PlayerStateId",
                principalTable: "PlayerStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId",
                principalTable: "ShipTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
