using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class serviceComment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ServiceComment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StayingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceComment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovingSchedule_ServiceComment",
                        column: x => x.MovingScheduleId,
                        principalTable: "MovingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StayingSchedule_ServiceComment",
                        column: x => x.StayingScheduleId,
                        principalTable: "StayingSchedule",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_User_ServiceComment",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ServiceComment_MovingScheduleId",
                table: "ServiceComment",
                column: "MovingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceComment_StayingScheduleId",
                table: "ServiceComment",
                column: "StayingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceComment_UserId",
                table: "ServiceComment",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ServiceComment");
        }
    }
}
