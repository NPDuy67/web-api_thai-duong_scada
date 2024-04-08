using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThaiDuongScada.Api.Migrations
{
    /// <inheritdoc />
    public partial class UpdateShiftTime : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftTime",
                table: "DeviceMoulds");

            migrationBuilder.AddColumn<int>(
                name: "ShiftDuration",
                table: "DeviceMoulds",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ShiftDuration",
                table: "DeviceMoulds");

            migrationBuilder.AddColumn<double>(
                name: "ShiftTime",
                table: "DeviceMoulds",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }
    }
}
