using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixNotify : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "MovingScheduleId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StayingScheduleId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ScheduleInterest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StayingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestStatus = table.Column<int>(type: "int", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScheduleInterest", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovingSchedule_ScheduleInterest",
                        column: x => x.MovingScheduleId,
                        principalTable: "MovingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StayingSchedule_ScheduleInterest",
                        column: x => x.StayingScheduleId,
                        principalTable: "StayingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_ScheduleInterest",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_MovingScheduleId",
                table: "Notification",
                column: "MovingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInterest_MovingScheduleId",
                table: "ScheduleInterest",
                column: "MovingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInterest_StayingScheduleId",
                table: "ScheduleInterest",
                column: "StayingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ScheduleInterest_UserId",
                table: "ScheduleInterest",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_Notification",
                table: "Notification",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification",
                column: "MovingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_Notification",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_Notification",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "ScheduleInterest");

            migrationBuilder.DropIndex(
                name: "IX_Notification_MovingScheduleId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "MovingScheduleId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "StayingScheduleId",
                table: "Notification");
        }
    }
}
