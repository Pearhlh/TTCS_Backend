using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class NotifyTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TourishPlanId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification",
                column: "TourishPlanId",
                unique: true,
                filter: "[TourishPlanId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "TourishPlanId",
                table: "Notification");
        }
    }
}
