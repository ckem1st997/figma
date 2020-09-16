using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class mesfb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FbMessage",
                table: "ConfigSites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FbMessage",
                table: "ConfigSites");
        }
    }
}
