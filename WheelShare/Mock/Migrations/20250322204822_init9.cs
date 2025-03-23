using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Mock.Migrations
{
    /// <inheritdoc />
    public partial class init9 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Seats",
                table: "Rides",
                newName: "NumSeats");

            migrationBuilder.RenameColumn(
                name: "IsPrivateRide",
                table: "Rides",
                newName: "SharedRide");

            migrationBuilder.AddColumn<string>(
                name: "DestinationAddress",
                table: "Rides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceAddress",
                table: "Rides",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DestinationAddress",
                table: "Rides");

            migrationBuilder.DropColumn(
                name: "SourceAddress",
                table: "Rides");

            migrationBuilder.RenameColumn(
                name: "SharedRide",
                table: "Rides",
                newName: "IsPrivateRide");

            migrationBuilder.RenameColumn(
                name: "NumSeats",
                table: "Rides",
                newName: "Seats");
        }
    }
}
