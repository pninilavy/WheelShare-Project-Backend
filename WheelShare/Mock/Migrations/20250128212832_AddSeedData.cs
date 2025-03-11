using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Address", "Area", "AvailableSpaces", "Capacity", "City" },
                values: new object[,]
                {
                    { 1, "Shmaaya 6", "Center", 2, 3, "Elad" },
                    { 2, "Hertzel St 15", "South", 1, 2, "Netivot" },
                    { 3, "Rabi Akiva 48", "Center", 0, 3, "Bnei-Brak" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "AvailabilityStatus", "LicensePlate", "Seats", "StationID" },
                values: new object[,]
                {
                    { 1, "Available", "12457963", 5, 1 },
                    { 2, "Unavailable", "32654526", 5, 1 },
                    { 3, "Available", "98563652", 7, 1 },
                    { 4, "Available", "48752165", 5, 2 },
                    { 5, "Unavailable", "98546532", 7, 2 },
                    { 6, "Unavailable", "32654896", 7, 3 },
                    { 7, "Unavailable", "12659745", 7, 3 },
                    { 8, "Unavailable", "92365498", 5, 3 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vehicles_StationID",
                table: "Vehicles",
                column: "StationID");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DestinationStationID",
                table: "Rides",
                column: "DestinationStationID");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_DriveId",
                table: "Rides",
                column: "DriveId");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_SourceStationID",
                table: "Rides",
                column: "SourceStationID");

            migrationBuilder.CreateIndex(
                name: "IX_Rides_VehicleId",
                table: "Rides",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_RideParticipants_RideId",
                table: "RideParticipants",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_RideParticipants_UserId",
                table: "RideParticipants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_RideId",
                table: "Payments",
                column: "RideId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_UserId",
                table: "Payments",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Rides_RideId",
                table: "Payments",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideParticipants_Rides_RideId",
                table: "RideParticipants",
                column: "RideId",
                principalTable: "Rides",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RideParticipants_Users_UserId",
                table: "RideParticipants",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Users_DriveId",
                table: "Rides",
                column: "DriveId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Rides_Vehicles_VehicleId",
                table: "Rides",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Vehicles_Stations_StationID",
                table: "Vehicles",
                column: "StationID",
                principalTable: "Stations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Rides_RideId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Users_UserId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_RideParticipants_Rides_RideId",
                table: "RideParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_RideParticipants_Users_UserId",
                table: "RideParticipants");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_DestinationStationID",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Stations_SourceStationID",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Users_DriveId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Rides_Vehicles_VehicleId",
                table: "Rides");

            migrationBuilder.DropForeignKey(
                name: "FK_Vehicles_Stations_StationID",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Vehicles_StationID",
                table: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DestinationStationID",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_DriveId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_SourceStationID",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_Rides_VehicleId",
                table: "Rides");

            migrationBuilder.DropIndex(
                name: "IX_RideParticipants_RideId",
                table: "RideParticipants");

            migrationBuilder.DropIndex(
                name: "IX_RideParticipants_UserId",
                table: "RideParticipants");

            migrationBuilder.DropIndex(
                name: "IX_Payments_RideId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_UserId",
                table: "Payments");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
