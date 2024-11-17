using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixCommentTour : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Content",
                table: "TourishComment");

            migrationBuilder.AddColumn<Guid>(
                name: "TourishPlanId",
                table: "TourishComment",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TourishComment_TourishPlanId",
                table: "TourishComment",
                column: "TourishPlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TourishPlan_TourishComment",
                table: "TourishComment",
                column: "TourishPlanId",
                principalTable: "TourishPlan",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TourishPlan_TourishComment",
                table: "TourishComment");

            migrationBuilder.DropIndex(
                name: "IX_TourishComment_TourishPlanId",
                table: "TourishComment");

            migrationBuilder.DropColumn(
                name: "TourishPlanId",
                table: "TourishComment");

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "TourishComment",
                type: "ntext",
                nullable: false,
                defaultValue: "");
        }
    }
}
