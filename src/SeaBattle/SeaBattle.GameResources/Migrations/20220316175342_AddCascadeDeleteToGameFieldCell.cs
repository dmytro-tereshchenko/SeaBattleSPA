using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class AddCascadeDeleteToGameFieldCell : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
