using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class mem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConfirmEmail",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "token",
                table: "Members");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "ConfirmEmail",
                table: "Members",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "token",
                table: "Members",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
