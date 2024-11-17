using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class dbFix2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminConId",
                table: "GuestMessageConHistory",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<DateTime>(
                name: "CloseDate",
                table: "GuestMessageConHistory",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory",
                column: "AdminConId",
                unique: true,
                filter: "[AdminConId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory");

            migrationBuilder.DropColumn(
                name: "CloseDate",
                table: "GuestMessageConHistory");

            migrationBuilder.AlterColumn<Guid>(
                name: "AdminConId",
                table: "GuestMessageConHistory",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory",
                column: "AdminConId",
                unique: true);
        }
    }
}
