using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class setForeign : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullScheduleReceipt_ServiceSchedule",
                table: "FullScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_ServiceSchedule",
                table: "ServiceSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_ServiceSchedule",
                table: "ServiceSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule");

            migrationBuilder.AddForeignKey(
                name: "FK_FullScheduleReceipt_ServiceSchedule",
                table: "FullScheduleReceipt",
                column: "ServiceScheduleId",
                principalTable: "ServiceSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_Instruction",
                table: "Instruction",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_Instruction",
                table: "Instruction",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_ScheduleInterest",
                table: "ScheduleInterest",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_ScheduleInterest",
                table: "ScheduleInterest",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_ServiceSchedule",
                table: "ServiceSchedule",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_ServiceSchedule",
                table: "ServiceSchedule",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullScheduleReceipt_ServiceSchedule",
                table: "FullScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_ScheduleInterest",
                table: "ScheduleInterest");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_ServiceSchedule",
                table: "ServiceSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_ServiceSchedule",
                table: "ServiceSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_MovingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_StayingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule");

            migrationBuilder.AddForeignKey(
                name: "FK_FullScheduleReceipt_ServiceSchedule",
                table: "FullScheduleReceipt",
                column: "ServiceScheduleId",
                principalTable: "ServiceSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_Instruction",
                table: "Instruction",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_Instruction",
                table: "Instruction",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_Instruction",
                table: "Instruction",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_ScheduleInterest",
                table: "ScheduleInterest",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_ScheduleInterest",
                table: "ScheduleInterest",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_ServiceSchedule",
                table: "ServiceSchedule",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_ServiceSchedule",
                table: "ServiceSchedule",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_MovingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt",
                column: "MovingScheduleId",
                principalTable: "MovingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_StayingSchedule_TotalScheduleReceipt",
                table: "TotalScheduleReceipt",
                column: "StayingScheduleId",
                principalTable: "StayingSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TourishSchedule",
                table: "TourishSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }
    }
}
