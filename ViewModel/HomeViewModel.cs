using figma.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace figma.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Banners> Banners { get; set; }
        public IEnumerable<Articles> Articles { get; set; }
        public IEnumerable<ViewProducts> Products { get; set; }
        public IEnumerable<Albums> Albums { get; set; }
        public IEnumerable<ConfigSites> ConfigSites { get; set; }
        public IEnumerable<ViewProducts> ViewProducts { get; set; }
        public IEnumerable<ItemBoxProductHome> ItemBoxProductHomes { get; set; }

        public class ItemBoxProductHome
        {
            public ProductCategories ProductCategory { get; set; }
            public IEnumerable<Products> Products { get; set; }
        }
    }
    public class GetSizeId
    {
        public string SizeProduc { get; set; }
    }
    public class GetColorId
    {
        public string NameColor { get; set; }
        public string Code { get; set; }
    }

    public class ViewProducts
    {
        public DateTime CreateDate { get; set; }
        public string Image { get; set; }
        public int ProductID { get; set; }
        public string Name { get; set; }
        public decimal SaleOff { get; set; }
        [DataType(DataType.Currency)]
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Price { get; set; }
        public bool Hot { get; set; }
        public int Sort { get; set; }
        public int Quantity { get; set; }
    }
    public class ProductDetailViewModel
    {
        public Products Product { get; set; }
        public IEnumerable<ProductLike> ProductLike { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Products> ViewProducts { get; set; }
        public ProductCategories RootCategory { get; set; }
        public Collection Collection { get; set; }
        public IEnumerable<TagProducts> TagProducts { get; set; }
        public IEnumerable<ProductSizeColor> ProductSizeColors { get; set; }
        public IEnumerable<Banners> Banners { get; set; }
        public IEnumerable<GetColorId> GetColors { get; set; }
        public IEnumerable<GetSizeId> GetSizes { get; set; }
    }

    public class ProductSCViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<ProductSizeColor> ProductSizeColors { get; set; }
    }

    public class ReviewViewModel
    {
        public Articles Article { get; set; }
        public IEnumerable<Articles> Articles { get; set; }
        public IEnumerable<ArticleCategorys> ListArticles { get; set; }

    }
}
