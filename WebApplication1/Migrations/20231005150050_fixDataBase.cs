using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class fixDataBase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "HeadquarterAddress",
                table: "PlaneAirline",
                newName: "HeadQuarterAddress");

            migrationBuilder.RenameColumn(
                name: "HeadquarterAddress",
                table: "PassengerCar",
                newName: "HeadQuarterAddress");

            migrationBuilder.AddColumn<Guid>(
                name: "RestaurantId",
                table: "EatSchedule",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantId",
                table: "EatSchedule");

            migrationBuilder.RenameColumn(
                name: "HeadQuarterAddress",
                table: "PlaneAirline",
                newName: "HeadquarterAddress");

            migrationBuilder.RenameColumn(
                name: "HeadQuarterAddress",
                table: "PassengerCar",
                newName: "HeadquarterAddress");
        }
    }
}
