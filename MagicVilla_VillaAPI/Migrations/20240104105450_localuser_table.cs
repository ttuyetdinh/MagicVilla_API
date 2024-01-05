using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class localuser_table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocalUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocalUsers", x => x.Id);
                });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6495), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6495) });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6497), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6498) });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6499), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6500) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6363), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6374) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6377), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6377) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6379), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6380) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6382), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6382) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6384), new DateTime(2024, 1, 4, 17, 54, 50, 181, DateTimeKind.Local).AddTicks(6385) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocalUsers");

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6050), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6050) });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6052), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6053) });

            migrationBuilder.UpdateData(
                table: "VillaNumbers",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6054), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(6054) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5891), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5900) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5903), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5904) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5906), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5906) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5908), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5908) });

            migrationBuilder.UpdateData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CreatedDate", "UpdatedDate" },
                values: new object[] { new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5910), new DateTime(2024, 1, 4, 17, 34, 15, 108, DateTimeKind.Local).AddTicks(5911) });
        }
    }
}
