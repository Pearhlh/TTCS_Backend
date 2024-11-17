using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "SinglePrice",
                table: "StayingSchedule",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "SinglePrice",
                table: "MovingSchedule",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TotalTicket",
                table: "FullReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "SinglePrice",
                table: "EatSchedule",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SinglePrice",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "SinglePrice",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "TotalTicket",
                table: "FullReceipt");

            migrationBuilder.DropColumn(
                name: "SinglePrice",
                table: "EatSchedule");
        }
    }
}
