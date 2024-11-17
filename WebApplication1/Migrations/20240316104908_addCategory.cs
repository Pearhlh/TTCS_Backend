using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class addCategory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreateDate",
                table: "TourishInterest",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.CreateTable(
                name: "TourishCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreateDate = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getutcdate()"),
                    UpdateDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourishCategory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TourishCategoryRelation",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TourishCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TourishCategoryRelation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TourishCategory_TourishCategoryRelation",
                        column: x => x.TourishCategoryId,
                        principalTable: "TourishCategory",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TourishPlan_TourishCategoryRelation",
                        column: x => x.TourishPlanId,
                        principalTable: "TourishPlan",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TourishCategoryRelation_TourishCategoryId",
                table: "TourishCategoryRelation",
                column: "TourishCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_TourishCategoryRelation_TourishPlanId",
                table: "TourishCategoryRelation",
                column: "TourishPlanId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TourishCategoryRelation");

            migrationBuilder.DropTable(
                name: "TourishCategory");

            migrationBuilder.DropColumn(
                name: "CreateDate",
                table: "TourishInterest");
        }
    }
}
