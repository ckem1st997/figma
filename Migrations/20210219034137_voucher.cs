using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class voucher : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Vouchers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Type = table.Column<bool>(nullable: false),
                    Condition = table.Column<bool>(nullable: false),
                    PriceUp = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    PriceDown = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    SumUse = table.Column<int>(nullable: false),
                    ReductionMax = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vouchers", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vouchers");
        }
    }
}
