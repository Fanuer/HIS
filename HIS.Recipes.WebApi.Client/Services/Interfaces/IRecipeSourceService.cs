using System;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.WebApi.Client.Services.Interfaces
{
    public interface IRecipeSourceService:IDisposable
    {
        /// <summary>
        /// Returns a list of all recipe sources
        /// </summary>
        /// <param name="searchterm">term to filter the tags</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        Task<ListViewModel<SourceListEntryViewModel>> GetSourcesAsync(string searchterm = "", int page = 0, int entriesPerPage = 10);

        /// <summary>
        /// Removes an existing source
        /// </summary>
        /// <param name="sourceId">Id of the recipe source</param>
        /// <returns></returns>
        Task RemoveSourceAsync(int sourceId);

        /// <summary>
        /// Creates a new Cookbook
        /// </summary>
        /// <param name="newCookbook">Data of the new cookbook</param>
        /// <returns></returns>
        Task<CookbookSourceViewModel> CreateCookbookAsync(CookbookSourceCreationViewModel newCookbook);

        /// <summary>
        /// Updates an existing cookbook
        /// </summary>
        /// <param name="cookbookId">Id of a cookbook</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        Task UpdateCookbookAsync(int cookbookId, CookbookSourceViewModel model);

        /// <summary>
        /// Adds or updates a cookbooksource of a recipe.
        /// Old source-data will be removed.
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="cookbookId">ID  of the cookbook</param>
        /// <param name="page">Page of the cookbook</param>
        /// <returns></returns>
        Task AddOrUpdateCookbookForRecipe(int recipeId, int cookbookId, int page);

        /// <summary>
        /// Overrides a recipes source with the data of a web source
        /// </summary>
        /// <param name="recipeId">Id of owning recipe</param>
        /// <param name="model">New source data</param>
        /// <returns></returns>
        Task<WebSourceViewModel> CreateWebSourceForRecipeAsync(int recipeId, WebSourceCreationViewModel model);

        /// <summary>
        /// Updates an existing web source of a recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <param name="sourceId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateWebSourceForRecipeAsync(int recipeId, int sourceId, WebSourceViewModel model);
    }
}