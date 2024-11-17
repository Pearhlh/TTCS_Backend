using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixReceipt1234 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FullScheduleReceipt_ServiceScheduleId",
                table: "FullScheduleReceipt");

            migrationBuilder.DropIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt");

            migrationBuilder.CreateIndex(
                name: "IX_FullScheduleReceipt_ServiceScheduleId",
                table: "FullScheduleReceipt",
                column: "ServiceScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt",
                column: "TourishScheduleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_FullScheduleReceipt_ServiceScheduleId",
                table: "FullScheduleReceipt");

            migrationBuilder.DropIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt");

            migrationBuilder.CreateIndex(
                name: "IX_FullScheduleReceipt_ServiceScheduleId",
                table: "FullScheduleReceipt",
                column: "ServiceScheduleId",
                unique: true,
                filter: "[ServiceScheduleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TourishScheduleId",
                table: "FullReceipt",
                column: "TourishScheduleId",
                unique: true,
                filter: "[TourishScheduleId] IS NOT NULL");
        }
    }
}
