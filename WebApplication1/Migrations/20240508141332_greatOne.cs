using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class greatOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "TotalReceipt");

            migrationBuilder.DropColumn(
                name: "ScheduleType",
                table: "TotalReceipt");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "StayingSchedule");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "MovingSchedule");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "EatSchedule");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "EatSchedule");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "EatSchedule");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "EatSchedule");

            migrationBuilder.CreateTable(
                name: "ServiceSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StayingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServiceSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MovingSchedule_ServiceSchedule",
                        column: x => x.MovingScheduleId,
                        principalTable: "MovingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StayingSchedule_ServiceSchedule",
                        column: x => x.StayingScheduleId,
                        principalTable: "StayingSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TotalScheduleReceipt",
                columns: table => new
                {
                    TotalReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MovingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StayingScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalScheduleReceipt", x => x.TotalReceiptId);
                    table.ForeignKey(
                        name: "FK_MovingSchedule_TotalScheduleReceipt",
                        column: x => x.MovingScheduleId,
                        principalTable: "MovingSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StayingSchedule_TotalScheduleReceipt",
                        column: x => x.StayingScheduleId,
                        principalTable: "StayingSchedule",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "FullScheduleReceipt",
                columns: table => new
                {
                    FullReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceScheduleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    GuestName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalPrice = table.Column<double>(type: "float", nullable: false),
                    TotalTicket = table.Column<int>(type: "int", nullable: false),
                    TotalChildTicket = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullScheduleReceipt", x => x.FullReceiptId);
                    table.ForeignKey(
                        name: "FK_FullScheduleReceipt_ServiceSchedule",
                        column: x => x.ServiceScheduleId,
                        principalTable: "ServiceSchedule",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_TotalScheduleReceipt_FullScheduleReceipt",
                        column: x => x.TotalReceiptId,
                        principalTable: "TotalScheduleReceipt",
                        principalColumn: "TotalReceiptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FullScheduleReceipt_ServiceScheduleId",
                table: "FullScheduleReceipt",
                column: "ServiceScheduleId",
                unique: true,
                filter: "[ServiceScheduleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_FullScheduleReceipt_TotalReceiptId",
                table: "FullScheduleReceipt",
                column: "TotalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedule_MovingScheduleId",
                table: "ServiceSchedule",
                column: "MovingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_ServiceSchedule_StayingScheduleId",
                table: "ServiceSchedule",
                column: "StayingScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalScheduleReceipt_MovingScheduleId",
                table: "TotalScheduleReceipt",
                column: "MovingScheduleId",
                unique: true,
                filter: "[MovingScheduleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_TotalScheduleReceipt_StayingScheduleId",
                table: "TotalScheduleReceipt",
                column: "StayingScheduleId",
                unique: true,
                filter: "[StayingScheduleId] IS NOT NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FullScheduleReceipt");

            migrationBuilder.DropTable(
                name: "ServiceSchedule");

            migrationBuilder.DropTable(
                name: "TotalScheduleReceipt");

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

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "StayingSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "StayingSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "StayingSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "StayingSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MovingSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "MovingSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "MovingSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "MovingSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "EatSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''");

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "EatSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "EatSchedule",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "EatSchedule",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
