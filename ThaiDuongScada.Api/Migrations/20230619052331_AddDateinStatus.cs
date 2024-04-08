using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiDuongScada.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDateinStatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "MachineStatus",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ShiftDuration",
                table: "MachineStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ShiftNumber",
                table: "MachineStatus",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "MachineStatus");

            migrationBuilder.DropColumn(
                name: "ShiftDuration",
                table: "MachineStatus");

            migrationBuilder.DropColumn(
                name: "ShiftNumber",
                table: "MachineStatus");
        }
    }
}
