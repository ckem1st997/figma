using figma.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace figma.ViewModel
{
    public class HeaderViewModel
    {
        public IEnumerable<ArticleCategory> ArticleCategories { get; set; }
        public IEnumerable<ProductCategories> ProductCategories { get; set; }
        public IEnumerable<Abouts> Abouts { get; set; }
        public IEnumerable<Carts> Carts { get; set; }
        public IEnumerable<ConfigSites> ConfigSites { get; set; }
    }
}
