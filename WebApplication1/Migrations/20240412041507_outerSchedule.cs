using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class outerSchedule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_EatSchedules",
                table: "EatSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_MovingSchedule",
                table: "MovingSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_StayingSchedules",
                table: "StayingSchedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "StayingSchedule",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "MovingSchedule",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "EatSchedule",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_EatSchedules",
                table: "EatSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_MovingSchedule",
                table: "MovingSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_StayingSchedules",
                table: "StayingSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_EatSchedules",
                table: "EatSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_MovingSchedule",
                table: "MovingSchedule");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_StayingSchedules",
                table: "StayingSchedule");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "StayingSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "MovingSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "EatSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_EatSchedules",
                table: "EatSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_MovingSchedule",
                table: "MovingSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_StayingSchedules",
                table: "StayingSchedule",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
