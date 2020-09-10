using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class mkl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "KeyWord",
                table: "Articles",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "KeyWord",
                table: "Articles");
        }
    }
}
