using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init13 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailabilityStatus",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "AvailableSpaces",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Stations");

            migrationBuilder.CreateTable(
                name: "VehicleAvailabilities",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    VehicleId = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartTime = table.Column<TimeSpan>(type: "time", nullable: false),
                    EndTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VehicleAvailabilities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VehicleAvailabilities_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Stations",
                columns: new[] { "Id", "Address", "Area", "City" },
                values: new object[,]
                {
                    { -10, "שמעיה 6", "מרכז", "אלעד" },
                    { -9, "הנביאים 54", "מרכז", "ירושלים" },
                    { -8, "דרך חברון 101", "מרכז", "ירושלים" },
                    { -7, "עזה 29", "מרכז", "ירושלים" },
                    { -6, "יפו 234", "מרכז", "ירושלים" },
                    { -5, "חזון איש 50", "מרכז", "בני ברק" },
                    { -4, "ז'בוטינסקי 150", "מרכז", "בני ברק" },
                    { -3, "רבי עקיבא 100", "מרכז", "בני ברק" },
                    { -2, "רבן יוחנן בן זכאי 97", "מרכז", "אלעד" },
                    { -1, "אבן גבירול 8", "מרכז", "אלעד" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "CostPerHour", "LicensePlate", "Seats", "StationID" },
                values: new object[,]
                {
                    { -33, 19.899999999999999, "666-34-789", 7, -9 },
                    { -32, 12.9, "666-34-678", 5, -9 },
                    { -31, 12.9, "666-34-567", 5, -9 },
                    { -30, 19.899999999999999, "555-34-789", 7, -8 },
                    { -29, 12.9, "555-34-678", 5, -8 },
                    { -28, 12.9, "555-34-567", 5, -8 },
                    { -27, 19.899999999999999, "555-34-456", 7, -8 },
                    { -26, 12.9, "555-23-678", 5, -8 },
                    { -25, 19.899999999999999, "444-78-789", 7, -7 },
                    { -24, 12.9, "444-56-789", 5, -7 },
                    { -23, 12.9, "444-23-789", 5, -7 },
                    { -22, 19.899999999999999, "444-34-789", 7, -7 },
                    { -21, 12.9, "444-34-678", 5, -6 },
                    { -20, 19.899999999999999, "333-34-789", 7, -5 },
                    { -19, 12.9, "333-34-678", 5, -5 },
                    { -18, 12.9, "333-34-567", 5, -5 },
                    { -17, 19.899999999999999, "333-34-456", 7, -5 },
                    { -16, 12.9, "333-23-678", 5, -5 },
                    { -15, 19.899999999999999, "222-34-789", 7, -4 },
                    { -14, 12.9, "222-34-678", 5, -4 },
                    { -13, 12.9, "222-34-567", 5, -4 },
                    { -12, 19.899999999999999, "111-23-789", 7, -3 },
                    { -11, 12.9, "111-23-678", 5, -3 },
                    { -10, 12.9, "111-23-567", 5, -3 },
                    { -9, 19.899999999999999, "111-23-456", 7, -3 },
                    { -8, 19.899999999999999, "234-56-788", 7, -2 },
                    { -7, 12.9, "234-56-789", 5, -2 },
                    { -6, 19.899999999999999, "345-67-890", 7, -2 },
                    { -5, 19.899999999999999, "234-56-788", 7, -1 },
                    { -4, 12.9, "234-56-789", 5, -1 },
                    { -3, 19.899999999999999, "123-45-677", 7, -10 },
                    { -2, 12.9, "123-45-679", 5, -10 },
                    { -1, 12.9, "123-45-678", 5, -10 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_VehicleAvailabilities_VehicleId",
                table: "VehicleAvailabilities",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "VehicleAvailabilities");

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -33);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -32);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -31);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -30);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -29);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -28);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -27);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -26);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -25);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -24);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -23);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -22);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -21);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -20);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -19);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -18);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -17);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -16);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -15);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -14);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -13);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -12);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -11);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Vehicles",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -10);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -9);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -8);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -7);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -6);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -5);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -4);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -3);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -2);

            migrationBuilder.DeleteData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -1);

            migrationBuilder.AddColumn<string>(
                name: "AvailabilityStatus",
                table: "Vehicles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "AvailableSpaces",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Stations",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
