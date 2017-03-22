using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.DB;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.Tests.Helper
{
    public abstract class ServiceTestBase:IDisposable
    {
        #region CONST
        #endregion

        #region FIELDS
        bool _disposed;
        #endregion

        #region CTOR
        ~ServiceTestBase()
        {
            Dispose(false);
        }
        #endregion

        #region METHODS

        protected virtual async Task InitializeAsync()
        {
            var options = new DbContextOptionsBuilder<RecipeDbContext>()
                .UseInMemoryDatabase("read_from_database")
                .Options;
            this.DbContext = new RecipeDbContext(options);

            var testDataCreationContext = new RecipeDbContext(options);
            this.TestData = new RepositoryTestData(testDataCreationContext);
            await this.TestData.CreateTestDataAsync(RepositoryTestData.TestDataRemoveOption.NoRemoval, false);
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
                if (this.TestData != null)
                {
                    this.TestData.TestDataCreated = false;
                }
                this.TestData?.Context.Database.EnsureDeleted();
                this.TestData?.Dispose();
                this.DbContext?.Dispose();
            }

            this.DbContext = null;

            _disposed = true;
        }

        #endregion

        #region PROPERTIES

        internal RecipeDbContext DbContext { get; private set; }
        internal RepositoryTestData TestData { get; private set; }
        #endregion
    }
}
