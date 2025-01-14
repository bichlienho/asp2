using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 569, DateTimeKind.Local).AddTicks(3364));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 569, DateTimeKind.Local).AddTicks(4232));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 569, DateTimeKind.Local).AddTicks(4237));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 571, DateTimeKind.Local).AddTicks(2175));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 571, DateTimeKind.Local).AddTicks(3355));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 12, 13, 38, 0, 571, DateTimeKind.Local).AddTicks(3360));
        }
    }
}
