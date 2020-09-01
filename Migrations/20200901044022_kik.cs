using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class kik : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    ArticleCategoryId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    CategorySort = table.Column<int>(nullable: false),
                    CategoryActive = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    ShowHome = table.Column<bool>(nullable: false),
                    ShowMenu = table.Column<bool>(nullable: false),
                    Slug = table.Column<string>(maxLength: 100, nullable: true),
                    Hot = table.Column<bool>(nullable: false),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.ArticleCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    View = table.Column<int>(nullable: false),
                    ArticleCategoryId = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Hot = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(maxLength: 300, nullable: true),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_ArticleCategoryId",
                        column: x => x.ArticleCategoryId,
                        principalTable: "ArticleCategories",
                        principalColumn: "ArticleCategoryId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategoryId",
                table: "Articles",
                column: "ArticleCategoryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "ArticleCategories");
        }
    }
}
