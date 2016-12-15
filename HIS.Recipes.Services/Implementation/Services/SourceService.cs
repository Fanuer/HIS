using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.Models;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Implementation.Services
{
    internal class SourceService : ISourceService
    {

        #region CONST

        #endregion

        #region FIELDS

        bool _disposed;
        private ILogger<SourceService> _log;
        private IMapper _mapper;
        private IRecipeSourceRepository _rep;
        private IRecipeRepository _recipeRep;
        private readonly BaseService<ICookbookSourceRepository, RecipeCookbookSource, CookbookSourceViewModel, CookbookSourceCreationViewModel> _cookbookAdapter;
        private readonly BaseService<IWebSourceRepository, RecipeUrlSource, WebSourceViewModel, WebSourceCreationViewModel> _webSourceAdapter;

        #endregion

        #region CTOR

        public SourceService(IRecipeSourceRepository rep, IRecipeRepository recipeRep, IMapper mapper, ILoggerFactory loggerFactory)
        {
            this._log = loggerFactory.CreateLogger<SourceService>();
            this._mapper = mapper;
            this._rep = rep;
            this._recipeRep = recipeRep;
            this._cookbookAdapter = new BaseService<ICookbookSourceRepository, RecipeCookbookSource, CookbookSourceViewModel, CookbookSourceCreationViewModel>(_rep.CookbookSources, mapper, this._log, "cookbook source");
            this._webSourceAdapter = new BaseService<IWebSourceRepository, RecipeUrlSource, WebSourceViewModel, WebSourceCreationViewModel>(_rep.WebSources, mapper, _log, "web source");
        }

        ~SourceService()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        public IQueryable<SourceListEntryViewModel> GetSources()
        {
            IQueryable<SourceListEntryViewModel> result;

            try
            {
                result = this._rep.BaseSources
                    .GetAll()
                    .Include(x => x.RecipeSourceRecipes)
                    .ProjectTo<SourceListEntryViewModel>(this._mapper.ConfigurationProvider);
                this._log.LogDebug(new EventId(), $"Returned all sources");
            }
            catch (Exception e)
            {
                this._log.LogError(new EventId(), e, $"Error on returning all sources");
                throw new Exception($"Error on returning all sources");
            }

            return result;
        }

        public IQueryable<CookbookSourceViewModel> GetCookbooks()
        {
            IQueryable<CookbookSourceViewModel> result;

            try
            {
                result = this._rep.CookbookSources
                    .GetAll()
                    .Include(x => x.RecipeSourceRecipes)
                    .ThenInclude(x => x.Recipe)
                    .ProjectTo<CookbookSourceViewModel>(this._mapper.ConfigurationProvider);
                this._log.LogDebug(new EventId(), $"Returned all cookbooks");
            }
            catch (Exception e)
            {
                this._log.LogError(new EventId(), e, $"Error on returning all cookbooks");
                throw new Exception($"Error on returning all cookbooks");
            }

            return result;
        }

        public async Task<CookbookSourceViewModel> AddCookbookAsync(CookbookSourceCreationViewModel creationModel)
        {
            return await this._cookbookAdapter.AddAsync(creationModel);
        }

        public async Task RemoveSourceAsync(int id)
        {
            try
            {
                var element = await this._rep.BaseSources.FindAsync(id);
                if (element == null)
                {
                    throw new DataObjectNotFoundException();
                }
                await this._rep.BaseSources.RemoveAsync(element);
                _log.LogDebug($"Source '{id}' successfully deleted");
            }
            catch (Exception e)
            {
                var message = $"An Error occured while deleting the Source '{id}'";
                _log.LogError(new EventId(), e, message);
                throw new Exception(message);
            }
        }

        public async Task UpdateCookbookAsync(int id, CookbookSourceViewModel model)
        {
            await this._cookbookAdapter.UpdateAsync(id, model);
        }

        public async Task<WebSourceViewModel> AddWebSourceAsync(int recipeId, WebSourceCreationViewModel source)
        {
            WebSourceViewModel result = null;
            try
            {
                if (source == null) { throw new ArgumentNullException(nameof(source)); }
                result = await this._webSourceAdapter.AddAsync(new WebSourceCreationViewModel() { Name = source.Name, SourceUrl = source.SourceUrl });
                var dbsource = await this._rep.WebSources.FindAsync(result.Id, x => x.RecipeSourceRecipes);
                var recipe = await this._recipeRep.FindAsync(recipeId, x => x.Source);
                if (recipe == null)
                {
                    throw new DataObjectNotFoundException($"No Recipe with id '{recipeId}' found");
                }
                recipe.Source = new RecipeSourceRecipe() { RecipeId = recipeId, SourceId = dbsource.Id };
                await this._recipeRep.SaveChangesAsync();
            }
            catch (Exception e)
            {
                var message = "An error on creating a web source";
                _log.LogError(new EventId(), e, message);
                throw new Exception(message);
            }
            return result;
        }

        public async Task UpdateWebSourceAsync(int recipeId, int sourceId, WebSourceViewModel source)
        {
            await this._webSourceAdapter.UpdateAsync(sourceId, source);
            var dbsource = await this._rep.WebSources.FindAsync(sourceId, x => x.RecipeSourceRecipes);
            var recipe = await this._recipeRep.FindAsync(recipeId, x => x.Source);
            if (recipe == null)
            {
                throw new DataObjectNotFoundException($"No Recipe with id '{recipeId}' found");
            }
            recipe.Source = new RecipeSourceRecipe() { RecipeId = recipeId, SourceId = dbsource.Id };
            await this._recipeRep.SaveChangesAsync();
        }

        public async Task<CookbookSourceViewModel> UpdateRecipeOnCookbookAsync(int recipeId, int sourceId, int page)
        {
            var dbsource = await this._rep.CookbookSources.FindAsync(sourceId, x=>x.RecipeSourceRecipes);
            var recipe = await this._recipeRep.FindAsync(recipeId, x => x.Source);

            if (dbsource == null)
            {
                throw new DataObjectNotFoundException($"No source with id '{sourceId}' found");
            }
            if (recipe == null)
            {
                throw new DataObjectNotFoundException($"No recipe with id '{recipeId}' found to add to cookbook {dbsource.Name} ");
            }
            // update
            if (dbsource.RecipeSourceRecipes.Any(x=>x.RecipeId.Equals(recipeId)) && recipe.SourceId.Equals(sourceId))
            {
                recipe.Source.Page = page;
            }
            else // create
            {
                recipe.SourceId = sourceId;
                recipe.Source = new RecipeSourceRecipe()
                {
                    Recipe = recipe,
                    RecipeId = recipeId,
                    Source = dbsource,
                    SourceId = sourceId,
                    Page = page
                };
            }
            await this._recipeRep.SaveChangesAsync();
            dbsource = await this._rep.CookbookSources.FindAsync(sourceId, x => x.RecipeSourceRecipes);
            return this._mapper.Map<CookbookSourceViewModel>(dbsource);
        }

        public async Task RemoveRecipeFromCookbookAsync(int recipeId, int sourceId)
        {
            var dbSource = await _rep.CookbookSources.GetAll().Include(x => x.RecipeSourceRecipes).SingleOrDefaultAsync(x => x.Id.Equals(sourceId));
            var recipeSource = dbSource?.RecipeSourceRecipes.FirstOrDefault(x => x.RecipeId.Equals(recipeId) && x.SourceId.Equals(recipeId));
            if (recipeSource != null)
            {
                dbSource.RecipeSourceRecipes.Remove(recipeSource);
                await this._recipeRep.SaveChangesAsync();
            }
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
                this._rep.Dispose();
                this._recipeRep.Dispose();
            }

            this._log = null;
            this._mapper = null;
            this._rep = null;
            this._recipeRep = null;
            _disposed = true;
        }
        
        #endregion

        #region PROPERTIES
        #endregion
    }
}
