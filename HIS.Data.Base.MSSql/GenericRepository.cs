using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.Models;
using HIS.Data.Base.Interfaces.SingleId;
using Microsoft.EntityFrameworkCore;

namespace HIS.Data.Base.MSSql
{
    public abstract class GenericDbRepository<T, TIdProperty> : IRepositoryAddAndDelete<T, TIdProperty>, IRepositoryFindAll<T>, IRepositoryFindSingle<T, TIdProperty>, IRepositoryUpdate<T, TIdProperty>, ICountAsync, IManualSaveChanges where T : class, IEntity<TIdProperty>
    {
        #region Field

        #endregion

        #region Ctor

        protected GenericDbRepository(DbContext ctx)
        {
            if (ctx == null) { throw new ArgumentNullException(nameof(ctx)); }

            DbContext = ctx;
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

        public virtual async Task<T> FindAsync(TIdProperty id)
        {
            var dbSet = this.DbContext.Set<T>();
            if (dbSet == null)
            {
                throw new ArgumentException($"No DBSet of type {typeof(T).Name} was found in dbContext {this.DbContext.GetType().Name}");
            }

            return await dbSet.SingleOrDefaultAsync(x => x.Id.Equals(id));
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

        #endregion

        #region Property

        protected DbContext DbContext { get;}
        #endregion

        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await this.SaveChangesAsync();
        }
    }
}
