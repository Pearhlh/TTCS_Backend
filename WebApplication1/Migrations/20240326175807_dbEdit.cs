using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class dbEdit : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification",
                column: "TourishPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_TourishPlanId",
                table: "Notification",
                column: "TourishPlanId",
                unique: true,
                filter: "[TourishPlanId] IS NOT NULL");
        }
    }
}
