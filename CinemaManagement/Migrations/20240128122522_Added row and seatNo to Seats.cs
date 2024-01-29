using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaManagement.Migrations
{
    /// <inheritdoc />
    public partial class AddedrowandseatNotoSeats : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "Seats",
                newName: "SeatNo");

            migrationBuilder.AddColumn<string>(
                name: "SeatRow",
                table: "Seats",
                type: "nvarchar(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SeatRowNumber",
                table: "Seats",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SeatRow",
                table: "Seats");

            migrationBuilder.DropColumn(
                name: "SeatRowNumber",
                table: "Seats");

            migrationBuilder.RenameColumn(
                name: "SeatNo",
                table: "Seats",
                newName: "SeatNumber");
        }
    }
}
