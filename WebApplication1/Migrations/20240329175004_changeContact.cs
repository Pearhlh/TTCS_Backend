using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class changeContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HomeStay");

            migrationBuilder.DropTable(
                name: "Hotel");

            migrationBuilder.DropTable(
                name: "PassengerCar");

            migrationBuilder.DropTable(
                name: "PlaneAirline");

            migrationBuilder.CreateTable(
                name: "MovingContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BranchName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    VehicleType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_MovingContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RestHouseContact",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    RestHouseType = table.Column<int>(type: "int", nullable: false),
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
                    table.PrimaryKey("PK_RestHouseContact", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MovingContact");

            migrationBuilder.DropTable(
                name: "RestHouseContact");

            migrationBuilder.CreateTable(
                name: "HomeStay",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlaceBranch = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
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
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(900)", maxLength: 900, nullable: false),
                    DiscountAmount = table.Column<double>(type: "float", nullable: false),
                    DiscountFloat = table.Column<float>(type: "real", nullable: false),
                    HeadQuarterAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HotlineNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SupportEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaneAirline", x => x.Id);
                });
        }
    }
}
