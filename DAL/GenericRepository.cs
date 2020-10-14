using figma.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace figma.Data
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ShopProductContext _context;
        internal DbSet<TEntity> dbSet;
        private readonly string cacheKey = $"{typeof(TEntity)}";
        private IMemoryCache _cache;

        public GenericRepository(ShopProductContext context, IMemoryCache cache)
        {
            _context = context;
            dbSet = context.Set<TEntity>();
            _cache = cache;
        }

        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int records = 0,
            string includeProperties = "")
        {
            if (!_cache.TryGetValue(cacheKey, out IEnumerable<TEntity> cachedList))
            {
                IQueryable<TEntity> query = dbSet;

                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (records > 0 && orderBy != null)
                {
                    query = orderBy(query).Take(records);
                }
                else if (orderBy != null && records == 0)
                {
                    query = orderBy(query);
                }
                else if (orderBy == null && records > 0)
                {
                    query = query.Take(records);
                }
                cachedList = query;
                var cacheEntryOptions = new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(1));

                _cache.Set(cacheKey, cachedList, cacheEntryOptions);
            }
            return cachedList.ToList();
        }
        //aync

        public async Task<IEnumerable<TEntity>> GetAync(
          Expression<Func<TEntity, bool>> filter = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int records = 0,
          string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;
            if (!_cache.TryGetValue(cacheKey, out IQueryable<TEntity> cachedList))
            {
                if (filter != null)
                {
                    query = query.Where(filter);
                }

                foreach (var includeProperty in includeProperties.Split
                    (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(includeProperty);
                }

                if (records > 0 && orderBy != null)
                {
                    query = orderBy(query).Take(records);
                }
                else if (orderBy != null && records == 0)
                {
                    query = orderBy(query);
                }
                else if (orderBy == null && records > 0)
                {
                    query = query.Take(records);
                }
                cachedList = query;
                _cache.Set(cacheKey, cachedList, new MemoryCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromSeconds(1)));
            }
            return await cachedList.ToListAsync();
        }



        public virtual TEntity GetByID(object id)
        {
            return dbSet.Find(id);
        }

        public virtual TEntity GetByNotID(string id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = dbSet.Find(id);
            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (_context.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            _context.Entry(entityToUpdate).State = EntityState.Modified;
        }

        //public async Task<IReadOnlyList<T>> GetAllAsync()
        //{
        //    if (!_cacheService(cacheTech).TryGet(cacheKey, out IReadOnlyList<T> cachedList))
        //    {
        //        cachedList = await _dbContext
        //         .Set<T>()
        //         .ToListAsync();
        //        _cacheService(cacheTech).Set(cacheKey, cachedList);
        //    }
        //    return cachedList;
        //}
        //private readonly static Cache cacheTech = Cache.Memory;
        //private readonly string cacheKey = $"{typeof(T)}";
        //private readonly ApplicationContext _dbContext;
        //private readonly Func<Cache, ICacheService> _cacheService;

        //public GenericRepositoryAsync(ApplicationContext dbContext, Func<Cache, ICacheService> cacheService)
        //{
        //    _dbContext = dbContext;
        //    _cacheService = cacheService;
        //}
    }
}

