using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeatherApp.Migrations
{
    /// <inheritdoc />
    public partial class MergeDateTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "DataPoint");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "DataPoint",
                newName: "DateTime");

            migrationBuilder.AlterColumn<int>(
                name: "Temperature",
                table: "DataPoint",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,18)");

            migrationBuilder.AlterColumn<int>(
                name: "Humidity",
                table: "DataPoint",
                type: "int",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,18)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "DataPoint",
                newName: "Time");

            migrationBuilder.AlterColumn<decimal>(
                name: "Temperature",
                table: "DataPoint",
                type: "decimal(2,18)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<decimal>(
                name: "Humidity",
                table: "DataPoint",
                type: "decimal(2,18)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "DataPoint",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}
