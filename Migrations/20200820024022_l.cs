using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class l : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Image",
                table: "Banners");

            migrationBuilder.AddColumn<string>(
                name: "CoverImage",
                table: "Banners",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CoverImage",
                table: "Banners");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Banners",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
            //add-migration <name of migration>
        }
    }
}
