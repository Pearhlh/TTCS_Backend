using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class removeStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "TourishPlan");

            migrationBuilder.DropColumn(
                name: "PlanStatus",
                table: "TourishPlan");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "TourishPlan");

            migrationBuilder.AddForeignKey(
                name: "FK_User_ScheduleInterest",
                table: "ScheduleInterest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "TourishPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "PlanStatus",
                table: "TourishPlan",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "TourishPlan",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddForeignKey(
                name: "FK_User_ScheduleInterest",
                table: "ScheduleInterest",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
