using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class InitialCreate1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserID",
                table: "ProductLikes");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "ProductLikes",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "ProductLikes");

            migrationBuilder.AddColumn<int>(
                name: "UserID",
                table: "ProductLikes",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
