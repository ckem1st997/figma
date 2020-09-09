using figma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        //public IEnumerable<Color> Colors { get; set; }
        //public IEnumerable<Size> Sizes { get; set; }
    }

    //public class ContactViewModel
    //{
    //    public ConfigSites ConfigSite { get; set; }
    //    public Contacts Contact { get; set; }
    //    public int Result { get; set; }
    //}
    //public class HeaderViewModel
    //{
    //    public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
    //    public IEnumerable<ProductCategories> ProductCategories { get; set; }
    //    public IEnumerable<Abouts> Abouts { get; set; }

    //    public IEnumerable<ConfigSites> ConfigSites { get; set; }
    //}
    //public class LeftAboutViewModel
    //{
    //    public int AboutId { get; set; }
    //    public IEnumerable<Abouts> Abouts { get; set; }
    //    public IEnumerable<Banners> Banners { get; set; }
    //}
    //public class ArticleCategoryDetailViewModel
    //{
    //    public ArticleCategory ArticleCategory { get; set; }

    //    public IEnumerable<Banners> Banners { get; set; }
    //    public IEnumerable<ArticleCategory> Categories { get; set; }
    //}


    //public class FooterViewModel
    //{
    //    public IEnumerable<Abouts> Abouts { get; set; }
    //    public IEnumerable<ProductCategories> ProductCategories { get; set; }
    //    public ConfigSites ConfigSite { get; set; }
    //}

    //public class AlbumViewModel
    //{
    //    public Albums Album { get; set; }
    //    public IEnumerable<Albums> Albums { get; set; }
    //}
}
