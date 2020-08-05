using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Models;

namespace figma.Data
{
    public static class DbInitializer
    {
        public static void Initialize(ShopProductContext context)
        {
            context.Database.EnsureCreated();

            // Look for any CategoryParents.
            if (context.Collections.Any())
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

            //var productcolor = new ProductSizeColor[]
            //{
            //new ProductSizeColor{ProductID=1,ColorID=1},
            //new ProductSizeColor{ProductID=2,SizeID=1},
            //new ProductSizeColor{ProductID=1,ColorID=1,SizeID=1},
            //};
            //foreach (ProductSizeColor s in productcolor)
            //{
            //    context.ProductSizeColors.Add(s);
            //}
            //context.SaveChanges()
            //
            var size = new Size[]
            {
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
            new ProductCategories{Name="Váy",Soft=1,Home=false,Active=true,TitleMeta="Váy đẹp, rẻ 2019"},
              new ProductCategories{Name="Váy Dạ Hội",ParentId=1,Soft=2,Home=false,Active=true,TitleMeta="Váy đẹp, rẻ 2019"},
                new ProductCategories{Name="Váy Dài",ParentId=1,Soft=3,Home=false,Active=true,TitleMeta="Váy đẹp, rẻ 2019"},
            };
            foreach (ProductCategories s in productCategories)
            {
                context.ProductCategories.Add(s);
            }
            context.SaveChanges();
        }
    }
}
