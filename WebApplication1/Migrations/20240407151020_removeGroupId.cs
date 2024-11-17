using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class removeGroupId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "GuestMessage");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage",
                column: "AdminMessageConId",
                principalTable: "AdminMessageCon",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage");

            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "GuestMessage",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage",
                column: "AdminMessageConId",
                principalTable: "AdminMessageCon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
