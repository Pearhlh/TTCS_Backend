using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class setTourishScheduleToFalse : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
