using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class DropPrimaryKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameFields_GameFieldId",
                table: "GameFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_GameFields_Games_GameId",
                table: "GameFields");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_GamePlayers_GamePlayerId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_Ships_ShipId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_StartFields_StartFieldId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipId",
                table: "StartFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_StartFields_StartFieldId",
                table: "StartFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GameFields_GameFieldId",
                table: "StartFields");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_Games_GameId",
                table: "StartFields");

            migrationBuilder.DropTable(
                name: "GamePlayerGame");

            migrationBuilder.DropTable(
                name: "GameShipRepair");

            migrationBuilder.DropTable(
                name: "GameShipWeapon");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartFields",
                table: "StartFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartFieldCells",
                table: "StartFieldCells");

            migrationBuilder.DropIndex(
                name: "IX_StartFieldCells_GameShipId",
                table: "StartFieldCells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ships",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Repairs",
                table: "Repairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameShips",
                table: "GameShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFields",
                table: "GameFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFieldCells",
                table: "GameFieldCells");

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 5L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Repairs",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 3L);

            migrationBuilder.DeleteData(
                table: "Ships",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 4L);

            migrationBuilder.DeleteData(
                table: "Weapons",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 2L);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "bigint",
                keyValue: 3L);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StartFields");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "StartFieldCells");

            migrationBuilder.DropColumn(
                name: "GameShipId",
                table: "StartFieldCells");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShipTypes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayerStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameShips");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameFields");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameFieldCells");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Weapons",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "GamePlayerId",
                table: "StartFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "StartFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "GameFieldId",
                table: "StartFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "StartFields",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "StartFieldId",
                table: "StartFieldCells",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "StartFieldCells",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "GameShipNewId",
                table: "StartFieldCells",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "ShipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "ShipTypeId",
                table: "Ships",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Repairs",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "PlayerStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "GameStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "StartFieldId",
                table: "GameShips",
                type: "int",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ShipId",
                table: "GameShips",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "GamePlayerId",
                table: "GameShips",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "GameShips",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "GameStateId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "PlayerStateId",
                table: "GamePlayers",
                type: "int",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldDefaultValue: 1L);

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "GamePlayers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "GameId",
                table: "GameFields",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "GameFields",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "GameShipId",
                table: "GameFieldCells",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<int>(
                name: "GameFieldId",
                table: "GameFieldCells",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "NewId",
                table: "GameFieldCells",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartFields",
                table: "StartFields",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartFieldCells",
                table: "StartFieldCells",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ships",
                table: "Ships",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Repairs",
                table: "Repairs",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameShips",
                table: "GameShips",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFields",
                table: "GameFields",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFieldCells",
                table: "GameFieldCells",
                column: "NewId");

            migrationBuilder.CreateIndex(
                name: "IX_StartFieldCells_GameShipNewId",
                table: "StartFieldCells",
                column: "GameShipNewId");

            migrationBuilder.AddForeignKey(
                name: "FK_GameFieldCells_GameFields_GameFieldId",
                table: "GameFieldCells",
                column: "GameFieldId",
                principalTable: "GameFields",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFields_Games_GameId",
                table: "GameFields",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                table: "GamePlayers",
                column: "PlayerStateId",
                principalTable: "PlayerStates",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games",
                column: "GameStateId",
                principalTable: "GameStates",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameShips_GamePlayers_GamePlayerId",
                table: "GameShips",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameShips_Ships_ShipId",
                table: "GameShips",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameShips_StartFields_StartFieldId",
                table: "GameShips",
                column: "StartFieldId",
                principalTable: "StartFields",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId",
                principalTable: "ShipTypes",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipNewId",
                table: "StartFieldCells",
                column: "GameShipNewId",
                principalTable: "GameShips",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_StartFields_StartFieldId",
                table: "StartFieldCells",
                column: "StartFieldId",
                principalTable: "StartFields",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GameFields_GameFieldId",
                table: "StartFields",
                column: "GameFieldId",
                principalTable: "GameFields",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_Games_GameId",
                table: "StartFields",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "NewId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameFields_GameFieldId",
                table: "GameFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_GameFields_Games_GameId",
                table: "GameFields");

            migrationBuilder.DropForeignKey(
                name: "FK_GamePlayers_PlayerStates_PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DropForeignKey(
                name: "FK_Games_GameStates_GameStateId",
                table: "Games");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_GamePlayers_GamePlayerId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_Ships_ShipId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_GameShips_StartFields_StartFieldId",
                table: "GameShips");

            migrationBuilder.DropForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipNewId",
                table: "StartFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFieldCells_StartFields_StartFieldId",
                table: "StartFieldCells");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GameFields_GameFieldId",
                table: "StartFields");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields");

            migrationBuilder.DropForeignKey(
                name: "FK_StartFields_Games_GameId",
                table: "StartFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartFields",
                table: "StartFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StartFieldCells",
                table: "StartFieldCells");

            migrationBuilder.DropIndex(
                name: "IX_StartFieldCells_GameShipNewId",
                table: "StartFieldCells");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Ships",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Repairs",
                table: "Repairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameShips",
                table: "GameShips");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Games",
                table: "Games");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFields",
                table: "GameFields");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameFieldCells",
                table: "GameFieldCells");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Weapons");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "StartFields");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "StartFieldCells");

            migrationBuilder.DropColumn(
                name: "GameShipNewId",
                table: "StartFieldCells");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "ShipTypes");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "PlayerStates");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GameShips");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GamePlayers");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GameFields");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GameFieldCells");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Weapons",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "GamePlayerId",
                table: "StartFields",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "StartFields",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "GameFieldId",
                table: "StartFields",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "StartFields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "StartFieldId",
                table: "StartFieldCells",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "StartFieldCells",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "GameShipId",
                table: "StartFieldCells",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "ShipTypes",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "ShipTypeId",
                table: "Ships",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Ships",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Repairs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "PlayerStates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GameStates",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "StartFieldId",
                table: "GameShips",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ShipId",
                table: "GameShips",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "GamePlayerId",
                table: "GameShips",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GameShips",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "GameStateId",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "Games",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "PlayerStateId",
                table: "GamePlayers",
                type: "bigint",
                nullable: false,
                defaultValue: 1L,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 1);

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GamePlayers",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "GameId",
                table: "GameFields",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GameFields",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AlterColumn<long>(
                name: "GameShipId",
                table: "GameFieldCells",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "GameFieldId",
                table: "GameFieldCells",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<long>(
                name: "Id",
                table: "GameFieldCells",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Weapons",
                table: "Weapons",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartFields",
                table: "StartFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StartFieldCells",
                table: "StartFieldCells",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Ships",
                table: "Ships",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Repairs",
                table: "Repairs",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameShips",
                table: "GameShips",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Games",
                table: "Games",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GamePlayers",
                table: "GamePlayers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFields",
                table: "GameFields",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameFieldCells",
                table: "GameFieldCells",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_StartFieldCells_GameShipId",
                table: "StartFieldCells",
                column: "GameShipId");

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
                name: "FK_GameFieldCells_GameFields_GameFieldId",
                table: "GameFieldCells",
                column: "GameFieldId",
                principalTable: "GameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFieldCells_GameShips_GameShipId",
                table: "GameFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameFields_Games_GameId",
                table: "GameFields",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_GameShips_GamePlayers_GamePlayerId",
                table: "GameShips",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameShips_Ships_ShipId",
                table: "GameShips",
                column: "ShipId",
                principalTable: "Ships",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_GameShips_StartFields_StartFieldId",
                table: "GameShips",
                column: "StartFieldId",
                principalTable: "StartFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ships_ShipTypes_ShipTypeId",
                table: "Ships",
                column: "ShipTypeId",
                principalTable: "ShipTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_GameShips_GameShipId",
                table: "StartFieldCells",
                column: "GameShipId",
                principalTable: "GameShips",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFieldCells_StartFields_StartFieldId",
                table: "StartFieldCells",
                column: "StartFieldId",
                principalTable: "StartFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GameFields_GameFieldId",
                table: "StartFields",
                column: "GameFieldId",
                principalTable: "GameFields",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_GamePlayers_GamePlayerId",
                table: "StartFields",
                column: "GamePlayerId",
                principalTable: "GamePlayers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StartFields_Games_GameId",
                table: "StartFields",
                column: "GameId",
                principalTable: "Games",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
