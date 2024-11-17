using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class initialDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveFile_Message_MessageId",
                table: "SaveFile");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "MessageCon");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StayingSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MovingSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FullReceipt",
                type: "nvarchar(900)",
                maxLength: 900,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EatSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserSentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserReceiveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserCon_UserMessage",
                        column: x => x.UserReceiveId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_SentMessage",
                        column: x => x.UserSentId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserMessageCon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserMessageCon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_MessageCon",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "GuestMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    GuestMessageConId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GuestMessageCon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestConHisId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GuestPhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ConnectionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestMessageCon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Guest_MessageCon",
                        column: x => x.AdminId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "GuestMessageConHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestConId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminConId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GuestMessageConHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                        column: x => x.AdminConId,
                        principalTable: "GuestMessageCon",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessage_GuestMessageConId",
                table: "GuestMessage",
                column: "GuestMessageConId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageCon_AdminId",
                table: "GuestMessageCon",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageCon_GuestConHisId",
                table: "GuestMessageCon",
                column: "GuestConHisId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory",
                column: "AdminConId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_UserReceiveId",
                table: "UserMessage",
                column: "UserReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessage_UserSentId",
                table: "UserMessage",
                column: "UserSentId");

            migrationBuilder.CreateIndex(
                name: "IX_UserMessageCon_UserId",
                table: "UserMessageCon",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveFile_UserMessage_MessageId",
                table: "SaveFile",
                column: "MessageId",
                principalTable: "UserMessage",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestCon_GuestMessage",
                table: "GuestMessage",
                column: "GuestMessageConId",
                principalTable: "GuestMessageCon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Guest",
                table: "GuestMessageCon",
                column: "GuestConHisId",
                principalTable: "GuestMessageConHistory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SaveFile_UserMessage_MessageId",
                table: "SaveFile");

            migrationBuilder.DropForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                table: "GuestMessageConHistory");

            migrationBuilder.DropTable(
                name: "GuestMessage");

            migrationBuilder.DropTable(
                name: "UserMessage");

            migrationBuilder.DropTable(
                name: "UserMessageCon");

            migrationBuilder.DropTable(
                name: "GuestMessageCon");

            migrationBuilder.DropTable(
                name: "GuestMessageConHistory");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EatSchedule");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "FullReceipt",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(900)",
                oldMaxLength: 900);

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserReceiveId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserSentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    GroupId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_ReceiveMessage",
                        column: x => x.UserReceiveId,
                        principalTable: "User",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_User_SentMessage",
                        column: x => x.UserSentId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "MessageCon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    ConnectionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageCon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_MessageCon",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserReceiveId",
                table: "Message",
                column: "UserReceiveId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_UserSentId",
                table: "Message",
                column: "UserSentId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageCon_UserId",
                table: "MessageCon",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_SaveFile_Message_MessageId",
                table: "SaveFile",
                column: "MessageId",
                principalTable: "Message",
                principalColumn: "Id");
        }
    }
}
