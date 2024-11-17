using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class notificationChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_User_Notification",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Notification",
                newName: "UserCreateId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                newName: "IX_Notification_UserCreateId");

            migrationBuilder.AddColumn<Guid>(
                name: "UserReceiveId",
                table: "Notification",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserReceiveId",
                table: "Notification",
                column: "UserReceiveId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserCreate_Notification",
                table: "Notification",
                column: "UserCreateId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserReceive_Notification",
                table: "Notification",
                column: "UserReceiveId",
                principalTable: "User",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserCreate_Notification",
                table: "Notification");

            migrationBuilder.DropForeignKey(
                name: "FK_UserReceive_Notification",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_UserReceiveId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "UserReceiveId",
                table: "Notification");

            migrationBuilder.RenameColumn(
                name: "UserCreateId",
                table: "Notification",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Notification_UserCreateId",
                table: "Notification",
                newName: "IX_Notification_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_User_Notification",
                table: "Notification",
                column: "UserId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
