using figma.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace figma.Interface
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> Get(
           Expression<Func<TEntity, bool>> filter = null,
           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int records = 0,
           string includeProperties = "");
        //aync
        Task<IEnumerable<TEntity>> GetAync(
                 Expression<Func<TEntity, bool>> filter = null,
                 Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, int records = 0,
                 string includeProperties = "");



        TEntity GetByID(object id);

        TEntity GetByNotID(string id);

        void Insert(TEntity entity);

        void Delete(object id);

        void Delete(TEntity entityToDelete);


        void Update(TEntity entityToUpdate);

    }
}
