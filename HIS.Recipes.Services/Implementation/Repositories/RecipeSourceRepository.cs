using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.Interfaces.Repositories;

namespace HIS.Recipes.Services.Implementation.Repositories
{
    internal class RecipeSourceRepository: IRecipeSourceRepository
    {

        #region CONST
        #endregion

        #region FIELDS
        bool _disposed;
        #endregion

        #region CTOR

        public RecipeSourceRepository(
            IWebSourceRepository webSources,
            IBaseSourceRepository baseSources,
            ICookbookSourceRepository cookbookSources
            )
        {
            this.BaseSources = baseSources;
            this.CookbookSources = cookbookSources;
            this.WebSources = webSources;
        }

        ~RecipeSourceRepository()
        {
            Dispose(false);
        }
        #endregion

        #region METHODS
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
                this.BaseSources.Dispose();
                this.CookbookSources.Dispose();
                this.WebSources.Dispose();
            }

            this.BaseSources = null;
            this.CookbookSources = null;
            this.WebSources = null;
            _disposed = true;
        }

        #endregion

        #region PROPERTIES
        public IWebSourceRepository WebSources { get; private set; }
        public IBaseSourceRepository BaseSources { get; private set; }
        public ICookbookSourceRepository CookbookSources { get; private set; }
        #endregion


    }
}
