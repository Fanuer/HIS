using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.Models;
using HIS.Data.Base.Interfaces.SingleId;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HIS.Data.Base.MSSql
{
    public abstract class GenericDbRepository<T, TIdProperty> 
        : IRepositoryAddAndDelete<T, TIdProperty>, 
          IRepositoryFindAll<T>, 
          IRepositoryFindSingle<T, TIdProperty>, 
          IRepositoryUpdate<T, TIdProperty>, 
          ICountAsync, 
          IManualSaveChanges, 
          IDisposable 
          where T : class, IEntity<TIdProperty>
    {
        #region Field

        bool _disposed;
        

        #endregion

        #region Ctor

        protected GenericDbRepository(DbContext ctx)
        {
            if (ctx == null) { throw new ArgumentNullException(nameof(ctx)); }

            DbContext = ctx;
        }
        ~GenericDbRepository()
        {
            Dispose(false);
        }
        #endregion

        #region Method
        public async Task<T> AddAsync(T model)
        {
            if (model == null) { throw new ArgumentNullException(nameof(model)); }

            try
            {
                var existingModel = await this.FindAsync(model.Id);
                if (existingModel != null)
                {
                    await RemoveAsync(existingModel);
                }
                this.DbContext.Set<T>().Add(model);
                await DbContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw new DbUpdateException($"Unable to add Entry of type {typeof(T).Namespace}", e);
            }
            return await this.FindAsync(model.Id);
        }

        public async Task<bool> RemoveAsync(TIdProperty id)
        {
            var model = await this.FindAsync(id);
            return await RemoveAsync(model);
        }

        public async Task<bool> RemoveAsync(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            this.DbContext.Set<T>().Remove(model);
            return await this.DbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> ExistsAsync(TIdProperty id)
        {
            return await this.GetAll().AnyAsync(e => e.Id.Equals(id));
        }

        public virtual IQueryable<T> GetAll()
        {
            return this.DbContext.Set<T>().AsQueryable();
        }

        
        public async Task<T> FindAsync<TProperty>(TIdProperty id, Expression<Func<T, TProperty>> navigationPropertyPath = null)
        {
            var dbSet = this.DbContext.Set<T>();
            if (dbSet == null)
            {
                throw new ArgumentException($"No DBSet of type {typeof(T).Name} was found in dbContext {this.DbContext.GetType().Name}");
            }
            
            return navigationPropertyPath != null ? await dbSet.Include(navigationPropertyPath).SingleOrDefaultAsync(x => x.Id.Equals(id)) : await dbSet.SingleOrDefaultAsync(x => x.Id.Equals(id));
        }

        public Task<T> FindAsync(TIdProperty id)
        {
            return this.FindAsync<object>(id);
        }

        public async Task<bool> UpdateAsync(T model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }
            this.DbContext.Entry(model).State = EntityState.Modified;
            return await this.DbContext.SaveChangesAsync() > 0;
        }

        public async Task<int> CountAsync()
        {
            return await DbContext.Set<T>().CountAsync();
        }

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }
        
        public async Task<int> SaveChangesAsync()
        {
            return await this.DbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Releases an returns unnessessary system resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this.DbContext.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null
            this.DbContext = null;
            _disposed = true;
        }
        #endregion

        #region Property

        protected DbContext DbContext { get; private set; }
        #endregion
    }
}
