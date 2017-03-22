using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Extensions;
using HIS.Helpers.Options;
using HIS.Recipes.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Gateway.Services.Clients.Recipe.Interfaces
{
    internal class RecipeSourceService : RmServiceBase
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public RecipeSourceService(IOptions<GatewayInformation> clientOptions, ILoggerFactory factory)
            : base(clientOptions, factory.CreateLogger<RecipeSourceService>())
        {
        }

        #endregion

        #region METHODS
        #region General
        /// <summary>
        /// Returns a list of all recipe sources
        /// </summary>
        /// <param name="searchterm">term to filter the tags</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        public async Task<ListViewModel<SourceListEntryViewModel>> GetSourcesAsync(string searchterm = "", int page = 0, int entriesPerPage = 10)
        {
            var uri = this.Client.AddToQueryString("Sources", new { searchterm, page, entriesPerPage });
            return await this.Client.GetAsync<ListViewModel<SourceListEntryViewModel>>(uri);
        }

        /// <summary>
        /// Removes an existing source
        /// </summary>
        /// <param name="sourceId">Id of the recipe source</param>
        /// <returns></returns>
        public async Task RemoveSourceAsync(int sourceId)
        {
            await this.Client.DeleteAsync($"Sources/{sourceId}");
        }

        #endregion

        #region Cookbook
        /// <summary>
        /// Creates a new Cookbook
        /// </summary>
        /// <param name="newCookbook">Data of the new cookbook</param>
        /// <returns></returns>
        public async Task<CookbookSourceViewModel> CreateCookbookAsync(CookbookSourceCreationViewModel newCookbook)
        {
            return await this.Client.PostAsJsonReturnAsync<CookbookSourceCreationViewModel, CookbookSourceViewModel>(newCookbook, "Cookbook");
        }

        /// <summary>
        /// Updates an existing cookbook
        /// </summary>
        /// <param name="cookbookId">Id of a cookbook</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        public async Task UpdateCookbookAsync(int cookbookId, CookbookSourceViewModel model)
        {
            await this.Client.PutAsJsonAsync(model, $"Cookbooks/{cookbookId}");
        }

        /// <summary>
        /// Adds or updates a cookbooksource of a recipe.
        /// Old source-data will be removed.
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="cookbookId">ID  of the cookbook</param>
        /// <param name="page">Page of the cookbook</param>
        /// <returns></returns>
        public async Task AddOrUpdateCookbookForRecipe(int recipeId, int cookbookId, int page)
        {
            await this.Client.PutAsync($"Recipes/{recipeId}/Cookbook/{cookbookId}/{page}");
        }

        #endregion

        #region WebSource

        /// <summary>
        /// Overrides a recipes source with the data of a web source
        /// </summary>
        /// <param name="recipeId">Id of owning recipe</param>
        /// <param name="model">New source data</param>
        /// <returns></returns>
        public async Task<WebSourceViewModel> CreateWebSourceForRecipeAsync(int recipeId, WebSourceCreationViewModel model)
        {
            return await this.Client.PostAsJsonReturnAsync<WebSourceCreationViewModel, WebSourceViewModel>(model, $"Recipes/{recipeId}/WebSource");
        }

        /// <summary>
        /// Updates an existing web source of a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateWebSourceForRecipeAsync(int recipeId, int sourceId, WebSourceViewModel model)
        {
            await this.Client.PutAsJsonAsync(model, $"Recipes/{recipeId}/WebSource/{sourceId}");
        }

        #endregion
        #endregion

        #region PROPERTIES
        #endregion
    }
}
