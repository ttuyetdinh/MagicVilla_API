using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MagicVilla_VillaAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedVillaTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Ocuppancy",
                table: "Villas",
                newName: "Occupancy");

            migrationBuilder.InsertData(
                table: "Villas",
                columns: new[] { "Id", "Age", "Amenity", "CreatedDate", "Details", "ImageUrl", "Name", "Occupancy", "Rate", "Sqft", "UpdateDate" },
                values: new object[,]
                {
                    { 1, 23, "", new DateTime(2023, 12, 25, 17, 18, 24, 727, DateTimeKind.Local).AddTicks(7466), "BaoThw", "https://dotnetmastery.com/bluevillaimages/villa3.jpg", "Royal Villa", 4, 200.0, 550, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, 23, "", new DateTime(2023, 12, 25, 17, 18, 24, 727, DateTimeKind.Local).AddTicks(7491), "Baotrxn", "https://dotnetmastery.com/bluevillaimages/villa1.jpg", "Premium Pool Villa", 4, 300.0, 550, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, 23, "", new DateTime(2023, 12, 25, 17, 18, 24, 727, DateTimeKind.Local).AddTicks(7494), "Hgh", "https://dotnetmastery.com/bluevillaimages/villa4.jpg", "Luxury Pool Villa", 4, 400.0, 750, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, 23, "", new DateTime(2023, 12, 25, 17, 18, 24, 727, DateTimeKind.Local).AddTicks(7497), "Lanvieee", "https://dotnetmastery.com/bluevillaimages/villa5.jpg", "Diamond Villa", 4, 550.0, 900, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, 23, "", new DateTime(2023, 12, 25, 17, 18, 24, 727, DateTimeKind.Local).AddTicks(7499), "yenle", "https://dotnetmastery.com/bluevillaimages/villa2.jpg", "Diamond Pool Villa", 4, 600.0, 1100, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Villas",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.RenameColumn(
                name: "Occupancy",
                table: "Villas",
                newName: "Ocuppancy");
        }
    }
}
