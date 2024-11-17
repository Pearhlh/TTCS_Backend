using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixInstruction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction");

            migrationBuilder.CreateIndex(
                name: "IX_Instruction_TourishPlanId",
                table: "Instruction",
                column: "TourishPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction");

            migrationBuilder.DropIndex(
                name: "IX_Instruction_TourishPlanId",
                table: "Instruction");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction",
                column: "StayingScheduleId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }
    }
}
