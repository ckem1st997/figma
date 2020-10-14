using figma.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Models;
using Microsoft.Extensions.Caching.Memory;

namespace figma.DAL
{
    public class UnitOfWork : IDisposable
    {
        private readonly ShopProductContext _context;
        private IMemoryCache _cache;
        public UnitOfWork(ShopProductContext shopProduct, IMemoryCache cache)
        {
            _context = shopProduct;
            _cache = cache;
        }

        private GenericRepository<Admins> _adminRepository;
        private GenericRepository<ArticleCategory> _artcategoryRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Banners> _bannerRepository;
        private GenericRepository<Contacts> _contactRepository;
        private GenericRepository<Tags> _tagRepository;
        private GenericRepository<TagProducts> _tagProductsRepository;
        private GenericRepository<Abouts> _aboutRepository;
        private GenericRepository<ConfigSites> _configRepository;
        private GenericRepository<Videos> _videoRepository;
        private GenericRepository<Albums> _albumRepository;
        private GenericRepository<ProductCategories> _productcategoryRepository;
        private GenericRepository<Products> _productRepository;
        private GenericRepository<Members> _memberRepository;
        private GenericRepository<Order> _orderRepository;
        private GenericRepository<OrderDetail> _orderdetailRepository;
        private GenericRepository<Carts> _cartRepository;
        private GenericRepository<Collection> _collectionRepository;
        private GenericRepository<ProductSizeColor> _productSCRepository;
        private GenericRepository<Size> _sizeRepository;
        private GenericRepository<Color> _colorRepository;

        public GenericRepository<Order> OrderRepository => _orderRepository ?? (_orderRepository = new GenericRepository<Order>(_context, _cache));
        public GenericRepository<OrderDetail> OrderDetailRepository => _orderdetailRepository ?? (_orderdetailRepository = new GenericRepository<OrderDetail>(_context, _cache));
        public GenericRepository<Carts> CartRepository => _cartRepository ?? (_cartRepository = new GenericRepository<Carts>(_context, _cache));
        public GenericRepository<Members> MemberRepository => _memberRepository ?? (_memberRepository = new GenericRepository<Members>(_context, _cache));
        public GenericRepository<Products> ProductRepository => _productRepository ?? (_productRepository = new GenericRepository<Products>(_context, _cache));
        public GenericRepository<ProductCategories> ProductCategoryRepository => _productcategoryRepository ?? (_productcategoryRepository = new GenericRepository<ProductCategories>(_context, _cache));
        public GenericRepository<Albums> AlbumRepository => _albumRepository ?? (_albumRepository = new GenericRepository<Albums>(_context, _cache));
        public GenericRepository<Videos> VideoRepository => _videoRepository ?? (_videoRepository = new GenericRepository<Videos>(_context, _cache));
        public GenericRepository<ConfigSites> ConfigSiteRepository => _configRepository ?? (_configRepository = new GenericRepository<ConfigSites>(_context, _cache));
        public GenericRepository<Abouts> AboutRepository => _aboutRepository ?? (_aboutRepository = new GenericRepository<Abouts>(_context, _cache));
        public GenericRepository<Tags> TagRepository => _tagRepository ?? (_tagRepository = new GenericRepository<Tags>(_context, _cache));
        public GenericRepository<TagProducts> TagsProductsRepository => _tagProductsRepository ?? (_tagProductsRepository = new GenericRepository<TagProducts>(_context, _cache));
        public GenericRepository<Contacts> ContactRepository => _contactRepository ?? (_contactRepository = new GenericRepository<Contacts>(_context, _cache));
        public GenericRepository<Banners> BannerRepository => _bannerRepository ?? (_bannerRepository = new GenericRepository<Banners>(_context, _cache));
        public GenericRepository<Article> ArticleRepository => _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context, _cache));
        public GenericRepository<Size> SizeRepository => _sizeRepository ?? (_sizeRepository = new GenericRepository<Size>(_context, _cache));
        public GenericRepository<Color> ColorRepository => _colorRepository ?? (_colorRepository = new GenericRepository<Color>(_context, _cache));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository => _artcategoryRepository ?? (_artcategoryRepository = new GenericRepository<ArticleCategory>(_context, _cache));
        public GenericRepository<Admins> AdminRepository => _adminRepository ?? (_adminRepository = new GenericRepository<Admins>(_context, _cache));
        public GenericRepository<ProductSizeColor> ProductSCRepository => _productSCRepository ?? (_productSCRepository = new GenericRepository<ProductSizeColor>(_context, _cache));
        public GenericRepository<Collection> CollectionRepository => _collectionRepository ?? (_collectionRepository = new GenericRepository<Collection>(_context, _cache));

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public void SaveNotAync()
        {
            _context.SaveChanges();
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
