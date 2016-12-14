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
    internal class SourceService : ISourceService, IDisposable
    {

        #region CONST
        #endregion

        #region FIELDS

        bool _disposed;
        private ILogger<SourceService> _log;
        private IMapper _mapper;
        private IRecipeSourceRepository _rep;
        private IRecipeRepository _recipeRep;

        #endregion

        #region CTOR

        public SourceService(IRecipeSourceRepository rep, IRecipeRepository recipeRep, IMapper mapper, ILoggerFactory loggerFactory)
        {
            this._log = loggerFactory.CreateLogger<SourceService>();
            this._mapper = mapper;
            this._rep = rep;
            this._recipeRep = recipeRep;
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
                                .ThenInclude(x=>x.Recipe)
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
                _log.LogError(new EventId(), e, $"An Error occured while deleting the Source '{id}'");
                throw new Exception($"An Error occured while deleting the Source '{id}'");
            }
        }

        public async Task UpdateCookbookAsync(int id, CookbookSourceViewModel model)
        {
            await this.UpdateAsync<ICookbookSourceRepository, RecipeCookbookSource, int, CookbookSourceViewModel>(this._rep.CookbookSources, id, model, "cookbook");
        }

        public async Task UpdateWebSourceAsync(int recipeId, int sourceId, WebSourceViewModel source)
        {
            await this.UpdateAsync<IWebSourceRepository, RecipeUrlSource, int, WebSourceViewModel>(this._rep.WebSources, sourceId, source, "web source");
            var dbsource = await this._rep.WebSources.FindAsync(sourceId);

            var recipe = await this._recipeRep.FindAsync(recipeId, x=>x.Source);
            if (!recipe.SourceId.Equals(sourceId))
            {
                recipe.SourceId = sourceId;
                recipe.Source.Recipe = recipe;
                recipe.Source.Source = dbsource;
                await this._recipeRep.SaveChangesAsync();
            }
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

        private async Task UpdateAsync<T, TEntity, TId, TVM>(T repository, TId id, TVM model, string entityName)
    where T : class, IRepositoryUpdate<TEntity, TId>, IRepositoryFindSingle<TEntity, TId>
    where TEntity : class, IEntity<TId>
    where TVM : IViewModelEntity<TId>
        {
            try
            {
                if (id.Equals(0))
                {
                    throw new ArgumentNullException(nameof(id));
                }
                if (model == null)
                {
                    throw new ArgumentNullException(nameof(model));
                }
                if (id.Equals(model.Id))
                {
                    throw new IdsNotIdenticalException();
                }

                var existingElement = await repository.FindAsync(id);
                var newObject = Mapper.Map(model, existingElement);
                var updateResult = await repository.UpdateAsync(newObject);
                _log.LogDebug(new EventId(), updateResult ? $"{entityName} {id} successfully updated" : $"No need to update step {id}");
            }
            catch (DbUpdateConcurrencyException e)
            {
                _log.LogWarning(new EventId(), e, $"No {entityName} with id {id} found");
                throw new DataObjectNotFoundException();
            }
            catch (Exception e)
            {
                _log.LogError(new EventId(), e, $"An Error occured while updating a {entityName}");
                throw new Exception($"An Error occured while updating a {entityName}");
            }
        }

        
        #endregion

        #region PROPERTIES
        #endregion
    }
}
