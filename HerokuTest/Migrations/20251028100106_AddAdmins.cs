using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace HerokuTest.Migrations
{
    /// <inheritdoc />
    public partial class AddAdmins : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "ChatId", "CreatedAt", "FirstName", "IsAdmin", "LastCommand", "LastName", "PhoneNumber", "Username" },
                values: new object[,]
                {
                    { 1L, 0L, new DateTime(2025, 10, 28, 10, 1, 5, 571, DateTimeKind.Utc).AddTicks(684), "", true, "", "", "", "Nx1ze" },
                    { 2L, 0L, new DateTime(2025, 10, 28, 10, 1, 5, 571, DateTimeKind.Utc).AddTicks(1517), "", true, "", "", "", "chudotovartajikistan" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1L);

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2L);
        }
    }
}
