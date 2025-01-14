using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Migrations
{
    /// <inheritdoc />
    public partial class AddpILOTTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pilots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Demo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pilots", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 514, DateTimeKind.Local).AddTicks(5328));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 514, DateTimeKind.Local).AddTicks(6166));

            migrationBuilder.UpdateData(
                table: "Account",
                keyColumn: "id",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 514, DateTimeKind.Local).AddTicks(6170));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 1,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 516, DateTimeKind.Local).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 2,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 516, DateTimeKind.Local).AddTicks(6100));

            migrationBuilder.UpdateData(
                table: "Airports",
                keyColumn: "AirportID",
                keyValue: 3,
                column: "CreateDate",
                value: new DateTime(2025, 1, 13, 11, 30, 27, 516, DateTimeKind.Local).AddTicks(6105));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Pilots");

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
    }
}
