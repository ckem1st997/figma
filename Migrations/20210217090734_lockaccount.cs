using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class lockaccount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LockAccount",
                table: "Members",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LockAccount",
                table: "Members");
        }
    }
}
