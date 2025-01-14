using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 607, DateTimeKind.Local).AddTicks(7723));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 607, DateTimeKind.Local).AddTicks(9452));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 607, DateTimeKind.Local).AddTicks(9460));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 611, DateTimeKind.Local).AddTicks(4682));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 611, DateTimeKind.Local).AddTicks(6934));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 52, 51, 611, DateTimeKind.Local).AddTicks(6947));
        }
    }
}
