using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class setForeign1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }
    }
}
