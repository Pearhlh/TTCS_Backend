using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class firstDbMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HomeStay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HomeStay", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hotel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hotel", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PassengerCar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadquarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PassengerCar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlaneAirline",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadquarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirline", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Restaurant",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Restaurant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourishPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GuestName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    TourName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartingPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EndPoint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanStatus = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourishPlan", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EatSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EatSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourishPlan_EatSchedules",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MovingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DriverName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    VehiclePlate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
                    TransportId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartingPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HeadingPlace = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MovingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourishPlan_MovingSchedule",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StayingSchedule",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceName = table.Column<string>(type: "nvarchar(70)", maxLength: 70, nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RestHouseType = table.Column<int>(type: "int", nullable: false),
                    RestHouseBranchId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(600)", maxLength: 600, nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StayingSchedule", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourishPlan_StayingSchedules",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TotalReceipt",
                columns: table => new
                {
                    TotalReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CompleteDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TotalReceipt", x => x.TotalReceiptId);
                    table.ForeignKey(
                        name: "FK_TourishPlan_TotalReceipt",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Message",
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
                    ConnectionID = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UserAgent = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Connected = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
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

            migrationBuilder.CreateTable(
                name: "Notification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notification", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_Notification",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "NotificationCon",
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
                    table.PrimaryKey("PK_NotificationCon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_NotificationCon",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RefreshToken",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenDescription = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    JwtId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsUsed = table.Column<bool>(type: "bit", nullable: false),
                    IsRevoked = table.Column<bool>(type: "bit", nullable: false),
                    IssueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpiredDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefreshToken", x => x.Id);
                    table.ForeignKey(
                        name: "FK_User_RefreshToken",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FullReceipt",
                columns: table => new
                {
                    FullReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalReceiptId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    ServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TotalNumber = table.Column<int>(type: "int", nullable: false),
                    SinglePrice = table.Column<double>(type: "float", nullable: false),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FullReceipt", x => x.FullReceiptId);
                    table.ForeignKey(
                        name: "FK_TotalReceipt_FullReceipt",
                        column: x => x.TotalReceiptId,
                        principalTable: "TotalReceipt",
                        principalColumn: "TotalReceiptId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SaveFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccessSourceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ResourceType = table.Column<int>(type: "int", nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    MessageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SaveFile", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SaveFile_Message_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Message",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_SaveFile_TourishPlan_TourishPlanId",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_EatSchedule_TourishPlanId",
                table: "EatSchedule",
                column: "TourishPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_FullReceipt_TotalReceiptId",
                table: "FullReceipt",
                column: "TotalReceiptId");

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

            migrationBuilder.CreateIndex(
                name: "IX_MovingSchedule_TourishPlanId",
                table: "MovingSchedule",
                column: "TourishPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_UserId",
                table: "Notification",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_NotificationCon_UserId",
                table: "NotificationCon",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_RefreshToken_UserId",
                table: "RefreshToken",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveFile_MessageId",
                table: "SaveFile",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_SaveFile_TourishPlanId",
                table: "SaveFile",
                column: "TourishPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_StayingSchedule_TourishPlanId",
                table: "StayingSchedule",
                column: "TourishPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_TotalReceipt_TourishPlanId",
                table: "TotalReceipt",
                column: "TourishPlanId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_UserName",
                table: "User",
                column: "UserName",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EatSchedule");

            migrationBuilder.DropTable(
                name: "FullReceipt");

            migrationBuilder.DropTable(
                name: "HomeStay");

            migrationBuilder.DropTable(
                name: "Hotel");

            migrationBuilder.DropTable(
                name: "MessageCon");

            migrationBuilder.DropTable(
                name: "MovingSchedule");

            migrationBuilder.DropTable(
                name: "Notification");

            migrationBuilder.DropTable(
                name: "NotificationCon");

            migrationBuilder.DropTable(
                name: "PassengerCar");

            migrationBuilder.DropTable(
                name: "PlaneAirline");

            migrationBuilder.DropTable(
                name: "RefreshToken");

            migrationBuilder.DropTable(
                name: "Restaurant");

            migrationBuilder.DropTable(
                name: "SaveFile");

            migrationBuilder.DropTable(
                name: "StayingSchedule");

            migrationBuilder.DropTable(
                name: "TotalReceipt");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "TourishPlan");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
