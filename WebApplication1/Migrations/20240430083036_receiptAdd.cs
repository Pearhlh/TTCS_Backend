using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class receiptAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TotalReceipt_TourishPlanId",
                table: "TotalReceipt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt");

            migrationBuilder.DropIndex(
                name: "IX_FullReceipt_TotalReceiptId",
                table: "FullReceipt");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "TotalReceipt",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "ScheduleId",
                table: "TotalReceipt",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ScheduleType",
                table: "TotalReceipt",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FullReceipt",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<int>(
                name: "TotalChildTicket",
                table: "FullReceipt",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TourishScheduleId",
                table: "FullReceipt",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt",
                column: "TotalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalReceipt_TourishPlanId",
                table: "TotalReceipt",
                column: "TourishPlanId",
                unique: true,
                filter: "[TourishPlanId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt",
                column: "TourishScheduleId",
                unique: true,
                filter: "[TourishScheduleId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_FullReceipt_TourishSchedule",
                table: "FullReceipt",
                column: "TourishScheduleId",
                principalTable: "TourishSchedule",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FullReceipt_TourishSchedule",
                table: "FullReceipt");

            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt");

            migrationBuilder.DropIndex(
                name: "IX_TotalReceipt_TourishPlanId",
                table: "TotalReceipt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt");

            migrationBuilder.DropIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "TotalReceipt");

            migrationBuilder.DropColumn(
                name: "ScheduleType",
                table: "TotalReceipt");

            migrationBuilder.DropColumn(
                name: "TotalChildTicket",
                table: "FullReceipt");

            migrationBuilder.DropColumn(
                name: "TourishScheduleId",
                table: "FullReceipt");

            migrationBuilder.AlterColumn<Guid>(
                name: "TourishPlanId",
                table: "TotalReceipt",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreatedDate",
                table: "FullReceipt",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt",
                column: "FullReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalReceipt_TourishPlanId",
                table: "TotalReceipt",
                column: "TourishPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TotalReceiptId",
                table: "FullReceipt",
                column: "TotalReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TotalReceipt",
                table: "TotalReceipt",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
