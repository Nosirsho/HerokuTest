using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HerokuTest.Migrations
{
    /// <inheritdoc />
    public partial class AddLastCommandColumnToAppUserTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LastCommand",
                table: "Users",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastCommand",
                table: "Users");
        }
    }
}
