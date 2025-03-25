using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init15 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_DestinationStationID",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_SourceStationID",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "SourceStationID",
                table: "Rides",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<int>(
                name: "DestinationStationID",
                table: "Rides",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Stations_DestinationStationID",
                table: "Rides",
                column: "DestinationStationID",
                principalTable: "Stations",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Stations_SourceStationID",
                table: "Rides",
                column: "SourceStationID",
                principalTable: "Stations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_DestinationStationID",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_SourceStationID",
                table: "Rides");

            migrationBuilder.AlterColumn<int>(
                name: "SourceStationID",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "DestinationStationID",
                table: "Rides",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Stations_DestinationStationID",
                table: "Rides",
                column: "DestinationStationID",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Stations_SourceStationID",
                table: "Rides",
                column: "SourceStationID",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
