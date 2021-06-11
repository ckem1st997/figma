using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using figma.Data;

namespace figma.CUnit
{
    public class TUnitOfWork : IUnitOfWork
    {
        private readonly ShopProductContext _context;
        public IProductRes Products { get; }

        public TUnitOfWork(ShopProductContext bookStoreDbContext,IProductRes catalogueRepository)
        {
            _context = bookStoreDbContext;
            Products = catalogueRepository;
        }



        public int Complete()
        {
            return _context.SaveChanges();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
        }
    }
}
