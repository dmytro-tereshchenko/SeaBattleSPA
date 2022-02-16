using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class RefactorGameShipWeaponRepair : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameShipRepair");

            migrationBuilder.DropTable(
                name: "GameShipWeapon");

            migrationBuilder.CreateTable(
                name: "EquippedRepairs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RepairId = table.Column<int>(type: "int", nullable: false),
                    GameShipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquippedRepairs", x => new { x.Id, x.GameShipId, x.RepairId });
                    table.ForeignKey(
                        name: "FK_EquippedRepairs_GameShips_GameShipId",
                        column: x => x.GameShipId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquippedRepairs_Repairs_RepairId",
                        column: x => x.RepairId,
                        principalTable: "Repairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EquippedWeapons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WeaponId = table.Column<int>(type: "int", nullable: false),
                    GameShipId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EquippedWeapons", x => new { x.Id, x.GameShipId, x.WeaponId });
                    table.ForeignKey(
                        name: "FK_EquippedWeapons_GameShips_GameShipId",
                        column: x => x.GameShipId,
                        principalTable: "GameShips",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EquippedWeapons_Weapons_WeaponId",
                        column: x => x.WeaponId,
                        principalTable: "Weapons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EquippedRepairs_GameShipId",
                table: "EquippedRepairs",
                column: "GameShipId");

            migrationBuilder.CreateIndex(
                name: "IX_EquippedRepairs_RepairId",
                table: "EquippedRepairs",
                column: "RepairId");

            migrationBuilder.CreateIndex(
                name: "IX_EquippedWeapons_GameShipId",
                table: "EquippedWeapons",
                column: "GameShipId");

            migrationBuilder.CreateIndex(
                name: "IX_EquippedWeapons_WeaponId",
                table: "EquippedWeapons",
                column: "WeaponId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EquippedRepairs");

            migrationBuilder.DropTable(
                name: "EquippedWeapons");

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

            migrationBuilder.CreateIndex(
                name: "IX_GameShipRepair_RepairsId",
                table: "GameShipRepair",
                column: "RepairsId");

            migrationBuilder.CreateIndex(
                name: "IX_GameShipWeapon_WeaponsId",
                table: "GameShipWeapon",
                column: "WeaponsId");
        }
    }
}
