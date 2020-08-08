using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class InitialCreate11 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Products",
                nullable: false,
                defaultValue: 1,
                oldClrType: typeof(int),
                oldType: "int");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Sort",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldDefaultValue: 1);
        }
    }
}
