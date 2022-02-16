using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class AddData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "Cost",
                table: "Ships",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.InsertData(
                table: "GameStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Created" },
                    { 2L, "SearchPlayers" },
                    { 3L, "Init" },
                    { 4L, "Process" },
                    { 5L, "Finished" }
                });

            migrationBuilder.InsertData(
                table: "PlayerStates",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Created" },
                    { 2L, "InitializeField" },
                    { 3L, "Ready" },
                    { 4L, "Process" }
                });

            migrationBuilder.InsertData(
                table: "Repairs",
                columns: new[] { "Id", "RepairPower", "RepairRange" },
                values: new object[] { 1L, 40, 10 });

            migrationBuilder.InsertData(
                table: "ShipTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1L, "Military" },
                    { 2L, "Auxiliary" },
                    { 3L, "Mixed" }
                });

            migrationBuilder.InsertData(
                table: "Weapons",
                columns: new[] { "Id", "AttackRange", "Damage" },
                values: new object[] { 1L, 10, 50 });

            migrationBuilder.InsertData(
                table: "Ships",
                columns: new[] { "Id", "Cost", "MaxHp", "ShipTypeId", "Size", "Speed" },
                values: new object[,]
                {
                    { 4L, 4000L, 400, 1L, (byte)4, (byte)1 },
                    { 2L, 2000L, 200, 2L, (byte)2, (byte)3 },
                    { 3L, 3000L, 300, 2L, (byte)3, (byte)2 },
                    { 1L, 1000L, 100, 3L, (byte)1, (byte)4 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Repairs",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyValue: 3L);

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "Ships");
        }
    }
}
