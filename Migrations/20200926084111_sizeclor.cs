using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class sizeclor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Color",
                table: "Carts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Size",
                table: "Carts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Color",
                table: "Carts");

            migrationBuilder.DropColumn(
                name: "Size",
                table: "Carts");
        }
    }
}
