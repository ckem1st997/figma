using figma.Models;
using System.Collections.Generic;

namespace figma.ViewModel
{
    public class HomeViewModel
    {
        public IEnumerable<Banners> Banners { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Albums> Albums { get; set; }
        public IEnumerable<ConfigSites> ConfigSites { get; set; }
        public IEnumerable<ItemBoxProductHome> ItemBoxProductHomes { get; set; }

        public class ItemBoxProductHome
        {
            public ProductCategories ProductCategory { get; set; }
            public IEnumerable<Products> Products { get; set; }
        }
    }
    public class ProductDetailViewModel
    {
        public Products Product { get; set; }
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<Products> ViewProducts { get; set; }
        public ProductCategories RootCategory { get; set; }
        public Collection Collection { get; set; }
        public IEnumerable<TagProducts> TagProducts { get; set; }
        public IEnumerable<ProductSizeColor> ProductSizeColors { get; set; }
        public IEnumerable<Banners> Banners { get; set; }
    }

    public class ProductSCViewModel
    {
        public IEnumerable<Products> Products { get; set; }
        public IEnumerable<ProductSizeColor> ProductSizeColors { get; set; }
    }

    public class ReviewViewModel
    {
        public Article Article { get; set; }
        public IEnumerable<Article> Articles { get; set; }
        public IEnumerable<ArticleCategory> ListArticles { get; set; }

    }
}
