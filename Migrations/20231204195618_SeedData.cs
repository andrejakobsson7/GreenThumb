using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GreenThumb.Migrations
{
    public partial class SeedData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Plants",
                columns: new[] { "id", "name", "plant_date" },
                values: new object[,]
                {
                    { 1, "Rhododendron", new DateTime(2023, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 2, "Autumn raspberries", new DateTime(2023, 4, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 3, "Summer raspberries", new DateTime(2022, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 4, "Strawberries", new DateTime(2022, 5, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 5, "Elderflower", new DateTime(2010, 10, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Instructions",
                columns: new[] { "id", "description", "plant_id" },
                values: new object[,]
                {
                    { 1, "Should be planted in sour soil", 1 },
                    { 2, "Requires a lot of water", 1 },
                    { 3, "Prune down to bottom each winter", 2 },
                    { 4, "Prune down fruit giving flower shoots after harvest", 3 },
                    { 5, "Plant in sunny location", 3 },
                    { 6, "Needs to be protected from birds", 3 },
                    { 7, "Should be replaced every 4-5 year", 4 },
                    { 8, "Requires a lot of pest control", 5 },
                    { 9, "Plant in sunny location", 5 },
                    { 10, "Don't fertilize during summer months", 5 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Instructions",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Plants",
                keyColumn: "id",
                keyValue: 5);
        }
    }
}
