using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace figma.Migrations
{
    public partial class up1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Abouts",
                columns: table => new
                {
                    AboutID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Image = table.Column<string>(maxLength: 500, nullable: true),
                    CoverImage = table.Column<string>(maxLength: 500, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.AboutID);
                });

            migrationBuilder.CreateTable(
                name: "Admins",
                columns: table => new
                {
                    AdminId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Role = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Admins", x => x.AdminId);
                });

            migrationBuilder.CreateTable(
                name: "Albums",
                columns: table => new
                {
                    AlbumID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    ListImage = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 100, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Sort = table.Column<int>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Albums", x => x.AlbumID);
                });

            migrationBuilder.CreateTable(
                name: "ArticleCategories",
                columns: table => new
                {
                    ArticleCategorieID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(maxLength: 50, nullable: false),
                    Link = table.Column<string>(maxLength: 500, nullable: true),
                    CategorySort = table.Column<int>(nullable: false),
                    CategoryActive = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: false),
                    ShowHome = table.Column<bool>(nullable: false),
                    ShowMenu = table.Column<bool>(nullable: false),
                    Slug = table.Column<string>(maxLength: 100, nullable: true),
                    Hot = table.Column<bool>(nullable: false),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArticleCategories", x => x.ArticleCategorieID);
                });

            migrationBuilder.CreateTable(
                name: "Banners",
                columns: table => new
                {
                    BannerID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BannerName = table.Column<string>(maxLength: 100, nullable: false),
                    Image = table.Column<string>(nullable: false),
                    Width = table.Column<int>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    GroupId = table.Column<int>(nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    Soft = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Banners", x => x.BannerID);
                });

            migrationBuilder.CreateTable(
                name: "Collections",
                columns: table => new
                {
                    CollectionID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Quantity = table.Column<int>(nullable: false),
                    Factory = table.Column<string>(maxLength: 500, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    Sort = table.Column<int>(nullable: false),
                    Hot = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    Content = table.Column<string>(nullable: true),
                    StatusProduct = table.Column<bool>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Collections", x => x.CollectionID);
                });

            migrationBuilder.CreateTable(
                name: "Colors",
                columns: table => new
                {
                    ColorID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Code = table.Column<string>(nullable: true),
                    NameColor = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Colors", x => x.ColorID);
                });

            migrationBuilder.CreateTable(
                name: "ConfigSites",
                columns: table => new
                {
                    ConfigSiteID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Facebook = table.Column<string>(maxLength: 500, nullable: true),
                    GooglePlus = table.Column<string>(maxLength: 500, nullable: true),
                    Youtube = table.Column<string>(maxLength: 500, nullable: true),
                    Linkedin = table.Column<string>(maxLength: 500, nullable: true),
                    Twitter = table.Column<string>(maxLength: 500, nullable: true),
                    GoogleAnalytics = table.Column<string>(maxLength: 4000, nullable: true),
                    LiveChat = table.Column<string>(maxLength: 4000, nullable: true),
                    GoogleMap = table.Column<string>(maxLength: 4000, nullable: true),
                    Title = table.Column<string>(maxLength: 200, nullable: true),
                    Description = table.Column<string>(maxLength: 500, nullable: true),
                    ContactInfo = table.Column<string>(nullable: true),
                    FooterInfo = table.Column<string>(nullable: true),
                    Hotline = table.Column<string>(maxLength: 50, nullable: true),
                    Email = table.Column<string>(maxLength: 100, nullable: true),
                    Logo = table.Column<string>(maxLength: 500, nullable: true),
                    SaleOffProgram = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConfigSites", x => x.ConfigSiteID);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    ContactID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fullname = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 300, nullable: false),
                    Mobile = table.Column<long>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Body = table.Column<string>(maxLength: 4000, nullable: false),
                    CreateDate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.ContactID);
                });

            migrationBuilder.CreateTable(
                name: "Members",
                columns: table => new
                {
                    MemberId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(maxLength: 50, nullable: false),
                    Password = table.Column<string>(nullable: false),
                    Fullname = table.Column<string>(maxLength: 50, nullable: false),
                    Address = table.Column<string>(maxLength: 200, nullable: true),
                    Mobile = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    HomePage = table.Column<string>(maxLength: 200, nullable: true),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Members", x => x.MemberId);
                });

            migrationBuilder.CreateTable(
                name: "Orders",
                columns: table => new
                {
                    OrdersID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MaDonHang = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    Payment = table.Column<bool>(nullable: false),
                    TypePay = table.Column<int>(nullable: false),
                    Transport = table.Column<int>(nullable: false),
                    TransportDate = table.Column<DateTime>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    OrderMemberId = table.Column<int>(nullable: false),
                    Viewed = table.Column<bool>(nullable: false),
                    CustomerInfo_Fullname = table.Column<string>(maxLength: 50, nullable: false),
                    CustomerInfo_Address = table.Column<string>(maxLength: 200, nullable: false),
                    CustomerInfo_Mobile = table.Column<string>(maxLength: 11, nullable: false),
                    CustomerInfo_Email = table.Column<string>(maxLength: 50, nullable: false),
                    CustomerInfo_Body = table.Column<string>(maxLength: 200, nullable: true),
                    CustomerInfo_Gender = table.Column<string>(maxLength: 10, nullable: true),
                    ThanhToanTruoc = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Orders", x => x.OrdersID);
                });

            migrationBuilder.CreateTable(
                name: "ProductCategories",
                columns: table => new
                {
                    ProductCategorieID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Image = table.Column<string>(maxLength: 500, nullable: false),
                    CoverImage = table.Column<string>(maxLength: 500, nullable: false),
                    Url = table.Column<string>(maxLength: 500, nullable: true),
                    Soft = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    ParentId = table.Column<int>(nullable: true),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCategories", x => x.ProductCategorieID);
                });

            migrationBuilder.CreateTable(
                name: "Sizes",
                columns: table => new
                {
                    SizeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SizeProduct = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sizes", x => x.SizeID);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    TagID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    Soft = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.TagID);
                });

            migrationBuilder.CreateTable(
                name: "Videos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Image = table.Column<string>(maxLength: 500, nullable: true),
                    VideoLink = table.Column<string>(nullable: true),
                    Body = table.Column<string>(nullable: true),
                    Active = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Soft = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Videos", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                columns: table => new
                {
                    ArticleID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Subject = table.Column<string>(maxLength: 100, nullable: false),
                    Description = table.Column<string>(maxLength: 500, nullable: false),
                    Body = table.Column<string>(nullable: true),
                    Image = table.Column<string>(nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false),
                    View = table.Column<int>(nullable: false),
                    ArticleCategorieID = table.Column<int>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    Hot = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Url = table.Column<string>(maxLength: 300, nullable: true),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.ArticleID);
                    table.ForeignKey(
                        name: "FK_Articles_ArticleCategories_ArticleCategorieID",
                        column: x => x.ArticleCategorieID,
                        principalTable: "ArticleCategories",
                        principalColumn: "ArticleCategorieID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    ProductID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: false),
                    Description = table.Column<string>(nullable: false),
                    Image = table.Column<string>(nullable: false),
                    Body = table.Column<string>(nullable: true),
                    ProductCategorieID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Factory = table.Column<string>(maxLength: 501, nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    SaleOff = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    QuyCach = table.Column<string>(maxLength: 500, nullable: true),
                    Sort = table.Column<int>(nullable: false, defaultValue: 1),
                    Hot = table.Column<bool>(nullable: false),
                    Home = table.Column<bool>(nullable: false),
                    Active = table.Column<bool>(nullable: false),
                    TitleMeta = table.Column<string>(maxLength: 100, nullable: true),
                    DescriptionMeta = table.Column<string>(nullable: true),
                    GiftInfo = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    StatusProduct = table.Column<bool>(nullable: false),
                    CollectionID = table.Column<int>(nullable: false),
                    BarCode = table.Column<string>(maxLength: 50, nullable: true),
                    CreateDate = table.Column<DateTime>(nullable: false, defaultValueSql: "getdate()"),
                    CreateBy = table.Column<string>(nullable: true, defaultValue: "admin")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK_Products_Collections_CollectionID",
                        column: x => x.CollectionID,
                        principalTable: "Collections",
                        principalColumn: "CollectionID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Products_ProductCategories_ProductCategorieID",
                        column: x => x.ProductCategorieID,
                        principalTable: "ProductCategories",
                        principalColumn: "ProductCategorieID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Carts",
                columns: table => new
                {
                    RecordID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CartID = table.Column<string>(nullable: true),
                    ProductID = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    Count = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Carts", x => x.RecordID);
                    table.ForeignKey(
                        name: "FK_Carts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OrderDetails",
                columns: table => new
                {
                    OrdersID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderDetails", x => new { x.OrdersID, x.ProductID });
                    table.ForeignKey(
                        name: "FK_OrderDetails_Orders_OrdersID",
                        column: x => x.OrdersID,
                        principalTable: "Orders",
                        principalColumn: "OrdersID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OrderDetails_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProductLikes",
                columns: table => new
                {
                    ProductLikeID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: false),
                    MemberId = table.Column<int>(nullable: false),
                    ProductsProductID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductLikes", x => x.ProductLikeID);
                    table.ForeignKey(
                        name: "FK_ProductLikes_Members_MemberId",
                        column: x => x.MemberId,
                        principalTable: "Members",
                        principalColumn: "MemberId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductLikes_Products_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ProductSizeColors",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductID = table.Column<int>(nullable: false),
                    ColorID = table.Column<int>(nullable: false),
                    SizeID = table.Column<int>(nullable: false),
                    ProductsProductID = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductSizeColors", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductSizeColors_Colors_ColorID",
                        column: x => x.ColorID,
                        principalTable: "Colors",
                        principalColumn: "ColorID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductSizeColors_Products_ProductsProductID",
                        column: x => x.ProductsProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProductSizeColors_Sizes_SizeID",
                        column: x => x.SizeID,
                        principalTable: "Sizes",
                        principalColumn: "SizeID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReviewProducts",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProductId = table.Column<int>(nullable: false),
                    NumberStart = table.Column<int>(nullable: false),
                    userID = table.Column<string>(nullable: true),
                    Content = table.Column<string>(nullable: true),
                    StatusReview = table.Column<bool>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReviewProducts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReviewProducts_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TagProducts",
                columns: table => new
                {
                    TagID = table.Column<int>(nullable: false),
                    ProductID = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TagProducts", x => new { x.ProductID, x.TagID });
                    table.ForeignKey(
                        name: "FK_TagProducts_Products_ProductID",
                        column: x => x.ProductID,
                        principalTable: "Products",
                        principalColumn: "ProductID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TagProducts_Tags_TagID",
                        column: x => x.TagID,
                        principalTable: "Tags",
                        principalColumn: "TagID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ArticleCategorieID",
                table: "Articles",
                column: "ArticleCategorieID");

            migrationBuilder.CreateIndex(
                name: "IX_Carts_ProductID",
                table: "Carts",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_OrderDetails_ProductID",
                table: "OrderDetails",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLikes_MemberId",
                table: "ProductLikes",
                column: "MemberId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductLikes_ProductsProductID",
                table: "ProductLikes",
                column: "ProductsProductID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CollectionID",
                table: "Products",
                column: "CollectionID");

            migrationBuilder.CreateIndex(
                name: "IX_Products_ProductCategorieID",
                table: "Products",
                column: "ProductCategorieID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizeColors_ColorID",
                table: "ProductSizeColors",
                column: "ColorID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizeColors_ProductsProductID",
                table: "ProductSizeColors",
                column: "ProductsProductID");

            migrationBuilder.CreateIndex(
                name: "IX_ProductSizeColors_SizeID",
                table: "ProductSizeColors",
                column: "SizeID");

            migrationBuilder.CreateIndex(
                name: "IX_ReviewProducts_ProductId",
                table: "ReviewProducts",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_TagProducts_TagID",
                table: "TagProducts",
                column: "TagID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Abouts");

            migrationBuilder.DropTable(
                name: "Admins");

            migrationBuilder.DropTable(
                name: "Albums");

            migrationBuilder.DropTable(
                name: "Articles");

            migrationBuilder.DropTable(
                name: "Banners");

            migrationBuilder.DropTable(
                name: "Carts");

            migrationBuilder.DropTable(
                name: "ConfigSites");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "OrderDetails");

            migrationBuilder.DropTable(
                name: "ProductLikes");

            migrationBuilder.DropTable(
                name: "ProductSizeColors");

            migrationBuilder.DropTable(
                name: "ReviewProducts");

            migrationBuilder.DropTable(
                name: "TagProducts");

            migrationBuilder.DropTable(
                name: "Videos");

            migrationBuilder.DropTable(
                name: "ArticleCategories");

            migrationBuilder.DropTable(
                name: "Orders");

            migrationBuilder.DropTable(
                name: "Members");

            migrationBuilder.DropTable(
                name: "Colors");

            migrationBuilder.DropTable(
                name: "Sizes");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Collections");

            migrationBuilder.DropTable(
                name: "ProductCategories");
        }
    }
}
