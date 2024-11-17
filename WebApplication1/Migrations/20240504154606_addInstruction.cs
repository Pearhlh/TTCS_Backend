using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class addInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Instruction",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    MovingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StayingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InstructionType = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "ntext", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Instruction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovingSchedule_Instruction",
                        column: x => x.MovingScheduleId,
                        principalTable: "MovingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StayingSchedule_Instruction",
                        column: x => x.StayingScheduleId,
                        principalTable: "StayingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TourishPlan_Instruction",
                        column: x => x.StayingScheduleId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Instruction_MovingScheduleId",
                table: "Instruction",
                column: "MovingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_Instruction_StayingScheduleId",
                table: "Instruction",
                column: "StayingScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Instruction");
        }
    }
}
