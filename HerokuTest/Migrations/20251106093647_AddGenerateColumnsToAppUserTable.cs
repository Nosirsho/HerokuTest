using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerokuTest.Migrations
{
    /// <inheritdoc />
    public partial class AddGenerateColumnsToAppUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GenAddressName",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "GenAddressNumber",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GenAddressName",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "GenAddressNumber",
                table: "Users");
        }
    }
}
