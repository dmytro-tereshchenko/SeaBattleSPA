using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class AddCascadeDeleteToGameShip : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquippedRepairs_GameShips_GameShipId",
                table: "EquippedRepairs");

            migrationBuilder.DropForeignKey(
                name: "FK_EquippedWeapons_GameShips_GameShipId",
                table: "EquippedWeapons");

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedRepairs_GameShips_GameShipId",
                table: "EquippedRepairs",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedWeapons_GameShips_GameShipId",
                table: "EquippedWeapons",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EquippedRepairs_GameShips_GameShipId",
                table: "EquippedRepairs");

            migrationBuilder.DropForeignKey(
                name: "FK_EquippedWeapons_GameShips_GameShipId",
                table: "EquippedWeapons");

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedRepairs_GameShips_GameShipId",
                table: "EquippedRepairs",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EquippedWeapons_GameShips_GameShipId",
                table: "EquippedWeapons",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
