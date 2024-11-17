using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixFullReceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt",
                column: "FullReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TotalReceiptId",
                table: "FullReceipt",
                column: "TotalReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt");

            migrationBuilder.DropIndex(
                name: "IX_FullReceipt_TotalReceiptId",
                table: "FullReceipt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FullReceipt",
                table: "FullReceipt",
                column: "TotalReceiptId");
        }
    }
}
