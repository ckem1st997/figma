using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class lokdl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionMeta",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMetaTitle",
                table: "Articles",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DescriptionMetaTitle",
                table: "Articles");

            migrationBuilder.AddColumn<string>(
                name: "DescriptionMeta",
                table: "Articles",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: true);
        }
    }
}
