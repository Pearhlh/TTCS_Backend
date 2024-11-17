using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GuestCon_GuestMessage",
                table: "GuestMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Guest",
                table: "GuestMessageCon");

            migrationBuilder.DropForeignKey(
                name: "FK_Guest_MessageCon",
                table: "GuestMessageCon");

            migrationBuilder.DropForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                table: "GuestMessageConHistory");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessageCon_AdminId",
                table: "GuestMessageCon");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessageCon_GuestConHisId",
                table: "GuestMessageCon");

            migrationBuilder.DropColumn(
                name: "AdminId",
                table: "GuestMessageCon");

            migrationBuilder.DropColumn(
                name: "GuestConHisId",
                table: "GuestMessageCon");

            migrationBuilder.AlterColumn<string>(
                name: "GuestPhoneNumber",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuestName",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "GuestEmail",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AdminMessageConId",
                table: "GuestMessage",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AdminMessageCon",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ConnectionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminMessageCon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_AdminMessageCon",
                        column: x => x.AdminId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory",
                column: "AdminConId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory",
                column: "GuestConId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessage_AdminMessageConId",
                table: "GuestMessage",
                column: "AdminMessageConId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminMessageCon_AdminId",
                table: "AdminMessageCon",
                column: "AdminId");

            migrationBuilder.AddForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage",
                column: "AdminMessageConId",
                principalTable: "AdminMessageCon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_MessageCon",
                table: "GuestMessage",
                column: "GuestMessageConId",
                principalTable: "GuestMessageCon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                table: "GuestMessageConHistory",
                column: "AdminConId",
                principalTable: "AdminMessageCon",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Guest",
                table: "GuestMessageConHistory",
                column: "GuestConId",
                principalTable: "GuestMessageCon",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Admin_MessageCon",
                table: "GuestMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_Guest_MessageCon",
                table: "GuestMessage");

            migrationBuilder.DropForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                table: "GuestMessageConHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Guest",
                table: "GuestMessageConHistory");

            migrationBuilder.DropTable(
                name: "AdminMessageCon");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessageConHistory_GuestConId",
                table: "GuestMessageConHistory");

            migrationBuilder.DropIndex(
                name: "IX_GuestMessage_AdminMessageConId",
                table: "GuestMessage");

            migrationBuilder.DropColumn(
                name: "AdminMessageConId",
                table: "GuestMessage");

            migrationBuilder.AlterColumn<string>(
                name: "GuestPhoneNumber",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GuestName",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "GuestEmail",
                table: "GuestMessageCon",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminId",
                table: "GuestMessageCon",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GuestConHisId",
                table: "GuestMessageCon",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageConHistory_AdminConId",
                table: "GuestMessageConHistory",
                column: "AdminConId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageCon_AdminId",
                table: "GuestMessageCon",
                column: "AdminId");

            migrationBuilder.CreateIndex(
                name: "IX_GuestMessageCon_GuestConHisId",
                table: "GuestMessageCon",
                column: "GuestConHisId",
                unique: true);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Guest_MessageCon",
                table: "GuestMessageCon",
                column: "AdminId",
                principalTable: "User",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GuestMessageCon_GuestMessageConHis_Admin",
                table: "GuestMessageConHistory",
                column: "AdminConId",
                principalTable: "GuestMessageCon",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
