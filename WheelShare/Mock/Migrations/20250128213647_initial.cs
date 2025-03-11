using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
