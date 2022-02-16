using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class RenameKeyForEnum : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_PlayerStates_NewPlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_NewGameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipTypes_NewShipTypeId",
                table: "Ships");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "ShipTypes",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewShipTypeId",
                table: "Ships",
                newName: "ShipTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ships_NewShipTypeId",
                table: "Ships",
                newName: "IX_Ships_ShipTypeId");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "PlayerStates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewId",
                table: "GameStates",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "NewGameStateId",
                table: "Games",
                newName: "GameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_NewGameStateId",
                table: "Games",
                newName: "IX_Games_GameStateId");

            migrationBuilder.RenameColumn(
                name: "NewPlayerStateId",
                table: "GamePlayers",
                newName: "PlayerStateId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_NewPlayerStateId",
                table: "GamePlayers",
                newName: "IX_GamePlayers_PlayerStateId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "ShipTypes",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "ShipTypeId",
                table: "Ships",
                newName: "NewShipTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ships",
                newName: "IX_Ships_NewShipTypeId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "PlayerStates",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "GameStates",
                newName: "NewId");

            migrationBuilder.RenameColumn(
                name: "GameStateId",
                table: "Games",
                newName: "NewGameStateId");

            migrationBuilder.RenameIndex(
                name: "IX_Games_GameStateId",
                table: "Games",
                newName: "IX_Games_NewGameStateId");

            migrationBuilder.RenameColumn(
                name: "PlayerStateId",
                table: "GamePlayers",
                newName: "NewPlayerStateId");

            migrationBuilder.RenameIndex(
                name: "IX_GamePlayers_PlayerStateId",
                table: "GamePlayers",
                newName: "IX_GamePlayers_NewPlayerStateId");

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_PlayerStates_NewPlayerStateId",
                table: "GamePlayers",
                column: "NewPlayerStateId",
                principalTable: "PlayerStates",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameStates_NewGameStateId",
                table: "Games",
                column: "NewGameStateId",
                principalTable: "GameStates",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipTypes_NewShipTypeId",
                table: "Ships",
                column: "NewShipTypeId",
                principalTable: "ShipTypes",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
