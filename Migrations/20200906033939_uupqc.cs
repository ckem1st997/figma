using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class uupqc : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "nameShopee",
                table: "ConfigSites",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "urlWeb",
                table: "ConfigSites",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "nameShopee",
                table: "ConfigSites");

            migrationBuilder.DropColumn(
                name: "urlWeb",
                table: "ConfigSites");
        }
    }
}
