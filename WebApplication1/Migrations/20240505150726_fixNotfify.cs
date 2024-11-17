using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixNotfify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_StayingScheduleId",
                table: "Notification",
                column: "StayingScheduleId");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_StayingScheduleId",
                table: "Notification");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification",
                column: "MovingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");
        }
    }
}
