using figma.Models;
using System.Collections.Generic;

namespace figma.ViewModel
{
    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategorys> ArticleCategories { get; set; }
        public IEnumerable<ProductCategories> ProductCategories { get; set; }
        public IEnumerable<Abouts> Abouts { get; set; }
        public IEnumerable<Carts> Carts { get; set; }
        public IEnumerable<ConfigSites> ConfigSites { get; set; }
    }
}
