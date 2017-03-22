using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Gateway.Services.Clients.Recipe.Interfaces
{
    internal interface IRecipeService
    {
        /// <summary>
        /// Gets Recipes from the Recipe-API
        /// </summary>
        /// <param name="searchModel">searchterm to filter</param>
        /// <param name="page">page for pagination</param>
        /// <param name="entriesPerPage">entries per page for pagination</param>
        /// <returns></returns>
        Task<ListViewModel<ShortRecipeViewModel>> GetRecipesAsync(RecipeSearchViewModel searchModel = null, int page = 0, int entriesPerPage = 10);

        /// <summary>
        /// Get full Recipe information
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <returns></returns>
        Task<FullRecipeViewModel> GetRecipeAsync(int recipeId);

        /// <summary>
        /// Removes a Recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <returns></returns>
        Task DeleteRecipeAsync(int recipeId);

        /// <summary>
        /// Changes an existing Recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="updatedModel">Model with new Data</param>
        /// <returns></returns>
        Task UpdateRecipeAsync(int recipeId, RecipeUpdateViewModel updatedModel);

        /// <summary>
        /// Creates a new Recipe
        /// </summary>
        /// <param name="model">Recipe-Data</param>
        /// <returns></returns>
        Task<RecipeUpdateViewModel> CreateRecipeAsync(RecipeCreationViewModel model);

        /// <summary>
        /// Starts a cooking of a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <returns></returns>
        Task StartCookingAsync(int recipeId);

        void Dispose();
    }
}