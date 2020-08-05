using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class InitialCreate2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Members",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLikes_MemberId",
                table: "ProductLikes",
                column: "MemberId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProductLikes_Members_MemberId",
                table: "ProductLikes",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "MemberId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProductLikes_Members_MemberId",
                table: "ProductLikes");

            migrationBuilder.DropIndex(
                name: "IX_ProductLikes_MemberId",
                table: "ProductLikes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Members",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Members");

            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Members",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Members",
                table: "Members",
                column: "MemberId");
        }
    }
}
