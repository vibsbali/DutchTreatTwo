using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DutchTreatTwo.Migrations
{
    public partial class seeding : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "Id", "OrderDate", "OrderNumber", "UserId" },
                values: new object[] { 1, new DateTime(2018, 10, 25, 9, 5, 59, 208, DateTimeKind.Utc), "12345", null });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "Id",
                keyValue: 1);
        }
    }
}
