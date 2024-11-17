using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class setToNullRelation1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "GuestConId",
                table: "GuestMessageConHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory",
                column: "GuestConId",
                unique: true,
                filter: "[GuestConId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "GuestConId",
                table: "GuestMessageConHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory",
                column: "GuestConId",
                unique: true);
        }
    }
}
