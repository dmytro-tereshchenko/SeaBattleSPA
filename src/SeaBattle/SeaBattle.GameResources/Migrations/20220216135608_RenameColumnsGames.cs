using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class RenameColumnsGames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Winner",
                table: "Games",
                newName: "WinnerId");

            migrationBuilder.RenameColumn(
                name: "CurrentGamePlayerMove",
                table: "Games",
                newName: "CurrentGamePlayerMoveId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "WinnerId",
                table: "Games",
                newName: "Winner");

            migrationBuilder.RenameColumn(
                name: "CurrentGamePlayerMoveId",
                table: "Games",
                newName: "CurrentGamePlayerMove");
        }
    }
}
