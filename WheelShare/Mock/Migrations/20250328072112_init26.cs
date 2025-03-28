using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init26 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Stations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Stations",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DestinationLatitude",
                table: "Rides",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "DestinationLongitude",
                table: "Rides",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SourceLatitude",
                table: "Rides",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SourceLongitude",
                table: "Rides",
                type: "float",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -10,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.051439862876322, 34.948015999999996 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -9,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 31.784003712931952, 35.22122717329276 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -8,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 31.755126321188925, 35.22159803281663 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -7,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 31.772101281022941, 35.213644776995174 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -6,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 31.789415617741206, 35.201225803979334 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -5,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.08015762377341, 34.834114903966878 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -4,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.092824318521515, 34.837161519309724 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -3,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.085854374927862, 34.83238070396672 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -2,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.048633105278249, 34.965196103968253 });

            migrationBuilder.UpdateData(
                table: "Stations",
                keyColumn: "Id",
                keyValue: -1,
                columns: new[] { "Latitude", "Longitude" },
                values: new object[] { 32.053557145139408, 34.957408026612057 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Stations");

            migrationBuilder.DropColumn(
                name: "DestinationLatitude",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "DestinationLongitude",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SourceLatitude",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SourceLongitude",
                table: "Rides");
        }
    }
}
