using Microsoft.EntityFrameworkCore.Migrations;

namespace SeaBattle.GameResources.Migrations
{
    public partial class ChangeTypeIdForEnums : Migration
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes");

            migrationBuilder.DropIndex(
                name: "IX_Ships_ShipTypeId",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_Games_GameStateId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "Id",
                keyColumnType: "int",
                keyValue: 3);

            migrationBuilder.DropColumn(
                name: "Id",
                table: "ShipTypes");

            migrationBuilder.DropColumn(
                name: "ShipTypeId",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PlayerStates");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "GameStateId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "PlayerStateId",
                table: "GamePlayers");

            migrationBuilder.AddColumn<short>(
                name: "NewId",
                table: "ShipTypes",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<short>(
                name: "NewShipTypeId",
                table: "Ships",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "NewId",
                table: "PlayerStates",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<short>(
                name: "NewId",
                table: "GameStates",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<int>(
                name: "Winner",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "CurrentGamePlayerMove",
                table: "Games",
                type: "int",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<short>(
                name: "NewGameStateId",
                table: "Games",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddColumn<short>(
                name: "NewPlayerStateId",
                table: "GamePlayers",
                type: "smallint",
                nullable: false,
                defaultValue: (short)1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates",
                column: "NewId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "NewId");

            migrationBuilder.InsertData(
                table: "GameStates",
                columns: new[] { "NewId", "Name" },
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
                columns: new[] { "NewId", "Name" },
                values: new object[,]
                {
                    { (short)1, "Created" },
                    { (short)2, "InitializeField" },
                    { (short)3, "Ready" },
                    { (short)4, "Process" }
                });

            migrationBuilder.InsertData(
                table: "ShipTypes",
                columns: new[] { "NewId", "Name" },
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
                column: "NewShipTypeId",
                value: (short)3);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "NewShipTypeId",
                value: (short)2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "NewShipTypeId",
                value: (short)2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4,
                column: "NewShipTypeId",
                value: (short)1);

            migrationBuilder.CreateIndex(
                name: "IX_Ships_NewShipTypeId",
                table: "Ships",
                column: "NewShipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Games_NewGameStateId",
                table: "Games",
                column: "NewGameStateId");

            migrationBuilder.CreateIndex(
                name: "IX_GamePlayers_NewPlayerStateId",
                table: "GamePlayers",
                column: "NewPlayerStateId");

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

        protected override void Down(MigrationBuilder migrationBuilder)
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

            migrationBuilder.DropPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes");

            migrationBuilder.DropIndex(
                name: "IX_Ships_NewShipTypeId",
                table: "Ships");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates");

            migrationBuilder.DropIndex(
                name: "IX_Games_NewGameStateId",
                table: "Games");

            migrationBuilder.DropIndex(
                name: "IX_GamePlayers_NewPlayerStateId",
                table: "GamePlayers");

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "GameStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)5);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)3);

            migrationBuilder.DeleteData(
                table: "PlayerStates",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)4);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)1);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)2);

            migrationBuilder.DeleteData(
                table: "ShipTypes",
                keyColumn: "NewId",
                keyColumnType: "smallint",
                keyValue: (short)3);

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "ShipTypes");

            migrationBuilder.DropColumn(
                name: "NewShipTypeId",
                table: "Ships");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "PlayerStates");

            migrationBuilder.DropColumn(
                name: "NewId",
                table: "GameStates");

            migrationBuilder.DropColumn(
                name: "NewGameStateId",
                table: "Games");

            migrationBuilder.DropColumn(
                name: "NewPlayerStateId",
                table: "GamePlayers");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "ShipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "ShipTypeId",
                table: "Ships",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "PlayerStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "GameStates",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AlterColumn<string>(
                name: "Winner",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrentGamePlayerMove",
                table: "Games",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "GameStateId",
                table: "Games",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddColumn<int>(
                name: "PlayerStateId",
                table: "GamePlayers",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AddPrimaryKey(
                name: "PK_ShipTypes",
                table: "ShipTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PlayerStates",
                table: "PlayerStates",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GameStates",
                table: "GameStates",
                column: "Id");

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
                table: "ShipTypes",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Military" },
                    { 2, "Auxiliary" },
                    { 3, "Mixed" }
                });

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 1,
                column: "ShipTypeId",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 2,
                column: "ShipTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 3,
                column: "ShipTypeId",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Ships",
                keyColumn: "Id",
                keyValue: 4,
                column: "ShipTypeId",
                value: 1);

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
