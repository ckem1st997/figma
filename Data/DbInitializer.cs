using System;
using System.Linq;
using figma.Models;

namespace figma.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ShopProductContext context)
        {
            context.Database.EnsureCreated();

            // Look for any CategoryParents.
            if (context.ProductCategories.Any())
            {
                return;   // DB has been seeded
            }
            // color
            var color = new Color[]
            {
            new Color{Code="0",NameColor="Không màu"},
            new Color{Code="#000000",NameColor="Đen"},
            new Color{Code="#FFFFFF",NameColor="Trắng"},
            new Color{Code="#FF0000",NameColor="Đỏ"},
            new Color{Code="#FFFF00",NameColor="Vàng"},
            new Color{Code="#00FF00",NameColor="Xanh lá cây"},
            };
            foreach (Color s in color)
            {
                context.Colors.Add(s);
            }
            context.SaveChanges();

            var size = new Size[]
            {
            new Size{SizeProduct="Không Size"},
            new Size{SizeProduct="M"},
            new Size{SizeProduct="L"},
            new Size{SizeProduct="XL"},
            new Size{SizeProduct="XXL"},
            new Size{SizeProduct="30"},
            new Size{SizeProduct="31"},
            };
            foreach (Size s in size)
            {
                context.Sizes.Add(s);
            }
            context.SaveChanges();
            //



            var colletion = new Collection[]
            {
            new Collection{Name="Bộ sưu tập mùa hè",Description="Bộ sưu tập mùa hè đẹp nhất năm",Image="image/1819.jpg",Quantity=1,Factory="Việt Nam",Price=1111111,Sort=1,Hot=false,Home=false,Active=true,TitleMeta="Bộ sưu tập mùa hè",StatusProduct=true,CreateDate=DateTime.Now},
            };
            foreach (Collection s in colletion)
            {
                context.Collections.Add(s);
            }
            context.SaveChanges();

            //
            var productCategories = new ProductCategories[]
            {
            new ProductCategories{Name="Váy",Image="https://ckeditor.com/apps/ckfinder/userfiles/files/Products/Rectangle%209.png",CoverImage="https://ckeditor.com/apps/ckfinder/userfiles/files/Products/Rectangle%209.png",Soft=1,Home=false,Active=true,TitleMeta="Váy đẹp, rẻ 2019"}
            };
            foreach (ProductCategories s in productCategories)
            {
                context.ProductCategories.Add(s);
            }
            context.SaveChanges();
        }
    }
}
