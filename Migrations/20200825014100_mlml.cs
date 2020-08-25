using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class mlml : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sort",
                table: "Banners");

            migrationBuilder.AddColumn<int>(
                name: "Soft",
                table: "Banners",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Soft",
                table: "Banners");

            migrationBuilder.AddColumn<int>(
                name: "Sort",
                table: "Banners",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
