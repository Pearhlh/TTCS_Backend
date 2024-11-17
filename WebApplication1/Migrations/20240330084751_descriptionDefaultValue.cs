using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TourishApi.Migrations
{
    /// <inheritdoc />
    public partial class descriptionDefaultValue : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TourishPlan",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StayingSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "StayingSchedule",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MovingSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "MovingSchedule",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EatSchedule",
                type: "ntext",
                nullable: true,
                defaultValueSql: "''",
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "EatSchedule",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "getutcdate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "TourishPlan",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldDefaultValueSql: "''");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "StayingSchedule",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldDefaultValueSql: "''");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "StayingSchedule",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "MovingSchedule",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldDefaultValueSql: "''");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "MovingSchedule",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "EatSchedule",
                type: "ntext",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "ntext",
                oldNullable: true,
                oldDefaultValueSql: "''");

            migrationBuilder.AlterColumn<DateTime>(
                name: "CreateDate",
                table: "EatSchedule",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "getutcdate()");
        }
    }
}
