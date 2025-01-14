using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Account",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Account",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 390, DateTimeKind.Local).AddTicks(9656));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 391, DateTimeKind.Local).AddTicks(594));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 391, DateTimeKind.Local).AddTicks(599));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 393, DateTimeKind.Local).AddTicks(225));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 393, DateTimeKind.Local).AddTicks(1493));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 16, 4, 52, 393, DateTimeKind.Local).AddTicks(1499));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "IdentityNumber",
                table: "Account",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(10)",
                oldMaxLength: 10,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Address",
                table: "Account",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 514, DateTimeKind.Local).AddTicks(7214));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 514, DateTimeKind.Local).AddTicks(8774));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 514, DateTimeKind.Local).AddTicks(8779));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 517, DateTimeKind.Local).AddTicks(391));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 517, DateTimeKind.Local).AddTicks(1721));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 14, 5, 28, 517, DateTimeKind.Local).AddTicks(1727));
        }
    }
}
