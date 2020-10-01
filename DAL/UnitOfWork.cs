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
        //  private GenericRepository<Orders> _orderRepository;
        //  private GenericRepository<OrderDetails> _orderdetailRepository;
        private GenericRepository<Carts> _cartRepository;
        private GenericRepository<Collection> _collectionRepository;
        private GenericRepository<ProductSizeColor> _productSCRepository;
        private GenericRepository<Size> _sizeRepository;
        private GenericRepository<Color> _colorRepository;

        //  public GenericRepository<Orders> OrderRepository => _orderRepository ?? (_orderRepository = new GenericRepository<Orders>(_context));
        //  public GenericRepository<OrderDetails> OrderDetailRepository => _orderdetailRepository ?? (_orderdetailRepository = new GenericRepository<OrderDetails>(_context));
        public GenericRepository<Carts> CartRepository => _cartRepository ?? (_cartRepository = new GenericRepository<Carts>(_context));
        public GenericRepository<Members> MemberRepository => _memberRepository ?? (_memberRepository = new GenericRepository<Members>(_context));
        public GenericRepository<Products> ProductRepository => _productRepository ?? (_productRepository = new GenericRepository<Products>(_context));
        public GenericRepository<ProductCategories> ProductCategoryRepository => _productcategoryRepository ?? (_productcategoryRepository = new GenericRepository<ProductCategories>(_context));
        public GenericRepository<Albums> AlbumRepository => _albumRepository ?? (_albumRepository = new GenericRepository<Albums>(_context));
        public GenericRepository<Videos> VideoRepository => _videoRepository ?? (_videoRepository = new GenericRepository<Videos>(_context));
        public GenericRepository<ConfigSites> ConfigSiteRepository => _configRepository ?? (_configRepository = new GenericRepository<ConfigSites>(_context));
        public GenericRepository<Abouts> AboutRepository => _aboutRepository ?? (_aboutRepository = new GenericRepository<Abouts>(_context));
        public GenericRepository<Tags> TagRepository => _tagRepository ?? (_tagRepository = new GenericRepository<Tags>(_context));
        public GenericRepository<TagProducts> TagsProductsRepository => _tagProductsRepository ?? (_tagProductsRepository = new GenericRepository<TagProducts>(_context));
        public GenericRepository<Contacts> ContactRepository => _contactRepository ?? (_contactRepository = new GenericRepository<Contacts>(_context));
        public GenericRepository<Banners> BannerRepository => _bannerRepository ?? (_bannerRepository = new GenericRepository<Banners>(_context));
        public GenericRepository<Article> ArticleRepository => _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context));
        public GenericRepository<Size> SizeRepository => _sizeRepository ?? (_sizeRepository = new GenericRepository<Size>(_context));
        public GenericRepository<Color> ColorRepository => _colorRepository ?? (_colorRepository = new GenericRepository<Color>(_context));
        public GenericRepository<ArticleCategory> ArticleCategoryRepository => _artcategoryRepository ?? (_artcategoryRepository = new GenericRepository<ArticleCategory>(_context));
        public GenericRepository<Admins> AdminRepository => _adminRepository ?? (_adminRepository = new GenericRepository<Admins>(_context));
        public GenericRepository<ProductSizeColor> ProductSCRepository => _productSCRepository ?? (_productSCRepository = new GenericRepository<ProductSizeColor>(_context));
        public GenericRepository<Collection> CollectionRepository => _collectionRepository ?? (_collectionRepository = new GenericRepository<Collection>(_context));

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
