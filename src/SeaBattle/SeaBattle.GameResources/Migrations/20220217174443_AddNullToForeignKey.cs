using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class AddNullToForeignKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields");

            migrationBuilder.AlterColumn<int>(
                name: "GamePlayerId",
                table: "StartFields",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "StartFieldId",
                table: "StartFieldCells",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields");

            migrationBuilder.AlterColumn<int>(
                name: "GamePlayerId",
                table: "StartFields",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "StartFieldId",
                table: "StartFieldCells",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
