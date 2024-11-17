using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class numberFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainTicket",
                table: "TourishPlan");

            migrationBuilder.DropColumn(
                name: "TotalTicket",
                table: "TourishPlan");

            migrationBuilder.AddColumn<int>(
                name: "RemainTicket",
                table: "TourishSchedule",
                type: "int",
                nullable: false,
                defaultValue: 20);

            migrationBuilder.AddColumn<int>(
                name: "TotalTicket",
                table: "TourishSchedule",
                type: "int",
                nullable: false,
                defaultValue: 20);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RemainTicket",
                table: "TourishSchedule");

            migrationBuilder.DropColumn(
                name: "TotalTicket",
                table: "TourishSchedule");

            migrationBuilder.AddColumn<int>(
                name: "RemainTicket",
                table: "TourishPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TotalTicket",
                table: "TourishPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
