using figma.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Models;

namespace figma.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly ShopProductContext _context;
        public UnitOfWork(ShopProductContext shopProduct)
        {
            _context = shopProduct;
        }
        private GenericRepository<Admins> _adminRepository;
        private GenericRepository<ArticleCategories> _artcategoryRepository;
        private GenericRepository<Articles> _articleRepository;
        private GenericRepository<Banners> _bannerRepository;
        private GenericRepository<Contacts> _contactRepository;
        private GenericRepository<Tags> _tagRepository;
        private GenericRepository<Abouts> _aboutRepository;
        private GenericRepository<ConfigSites> _configRepository;
        private GenericRepository<Videos> _videoRepository;
        private GenericRepository<Albums> _albumRepository;
        private GenericRepository<ProductCategories> _productcategoryRepository;
        private GenericRepository<Products> _productRepository;
        private GenericRepository<Members> _memberRepository;
        private GenericRepository<Orders> _orderRepository;
        private GenericRepository<OrderDetails> _orderdetailRepository;
        private GenericRepository<Carts> _cartRepository;

        public GenericRepository<Orders> OrderRepository => _orderRepository ?? (_orderRepository = new GenericRepository<Orders>(_context));
        public GenericRepository<OrderDetails> OrderDetailRepository => _orderdetailRepository ?? (_orderdetailRepository = new GenericRepository<OrderDetails>(_context));
        public GenericRepository<Carts> CartRepository => _cartRepository ?? (_cartRepository = new GenericRepository<Carts>(_context));
        public GenericRepository<Members> MemberRepository => _memberRepository ?? (_memberRepository = new GenericRepository<Members>(_context));
        public GenericRepository<Products> ProductRepository => _productRepository ?? (_productRepository = new GenericRepository<Products>(_context));
        public GenericRepository<ProductCategories> ProductCategoryRepository => _productcategoryRepository ?? (_productcategoryRepository = new GenericRepository<ProductCategories>(_context));
        public GenericRepository<Albums> AlbumRepository => _albumRepository ?? (_albumRepository = new GenericRepository<Albums>(_context));
        public GenericRepository<Videos> VideoRepository => _videoRepository ?? (_videoRepository = new GenericRepository<Videos>(_context));

        public GenericRepository<ConfigSites> ConfigSiteRepository => _configRepository ?? (_configRepository = new GenericRepository<ConfigSites>(_context));

        public GenericRepository<Abouts> AboutRepository => _aboutRepository ?? (_aboutRepository = new GenericRepository<Abouts>(_context));

        public GenericRepository<Tags> TagRepository => _tagRepository ?? (_tagRepository = new GenericRepository<Tags>(_context));

        public GenericRepository<Contacts> ContactRepository => _contactRepository ?? (_contactRepository = new GenericRepository<Contacts>(_context));

        public GenericRepository<Banners> BannerRepository => _bannerRepository ?? (_bannerRepository = new GenericRepository<Banners>(_context));

        public GenericRepository<Articles> ArticleRepository => _articleRepository ?? (_articleRepository = new GenericRepository<Articles>(_context));

        public GenericRepository<ArticleCategories> ArticleCategoryRepository => _artcategoryRepository ?? (_artcategoryRepository = new GenericRepository<ArticleCategories>(_context));

        public GenericRepository<Admins> AdminRepository => _adminRepository ?? (_adminRepository = new GenericRepository<Admins>(_context));

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        private bool _disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

    }
}
