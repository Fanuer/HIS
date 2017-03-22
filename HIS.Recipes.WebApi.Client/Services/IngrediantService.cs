using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Helpers.Extensions;
using HIS.Helpers.Options;
using HIS.Helpers.Web.Clients;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.WebApi.Client.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Recipes.WebApi.Client.Services
{
    internal class IngrediantService: S2SClientBase, IIngrediantsService
    {
        public IngrediantService(IOptions<ClientInformation> clientOptions, ILoggerFactory loggerFactory) 
            : base(clientOptions.Value, loggerFactory?.CreateLogger<IngrediantService>())
        {
        }

        #region General CRUD

        /// <summary>
        /// Get all Ingrediants from the recipe api
        /// </summary>
        /// <param name="searchterm">term to filter the ingrediants</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        public async Task<ListViewModel<IngrediantStatisticViewModel>> GetAllIngrediantsAsync(string searchterm = "", int page = 0, int entriesPerPage = 10)
        {
            var uri = this.Client.AddToQueryString("Ingrediants", new { searchterm, page, entriesPerPage });
            return await this.Client.GetAsync<ListViewModel<IngrediantStatisticViewModel>>(uri);
        }

        /// <summary>
        /// Creates a new Ingrediants
        /// </summary>
        /// <param name="ingrediantName">Name of the Ingrediant</param>
        /// <returns></returns>
        public async Task<IngrediantViewModel> AddIngrediantsAsync(string ingrediantName)
        {
            return await this.Client.PostAsJsonReturnAsync<string, IngrediantViewModel>(ingrediantName, "Ingrediants");
        }

        /// <summary>
        /// Removes an Ingrediants
        /// </summary>
        /// <param name="ingrediantId">ID of an ingrediant</param>
        /// <returns></returns>
        public async Task DeleteIngrediantAsync(int ingrediantId)
        {
            await this.Client.DeleteAsync($"Ingrediant/{ingrediantId}");
        }

        /// <summary>
        /// Updates an Ingrediant
        /// </summary>
        /// <param name="ingrediantId">Id of the ingrediant to update</param>
        /// <param name="updateModel">new ingrediant data</param>
        /// <returns></returns>
        public async Task UpdateIngrediantAsync(int ingrediantId, NamedViewModel updateModel)
        {
            await this.Client.PutAsJsonAsync(updateModel, $"Ingrediant/{ingrediantId}");
        }

        #endregion

        #region Recipe specific

        /// <summary>
        /// Returns all Ingrediants of a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <returns></returns>
        public async Task<IEnumerable<IngrediantViewModel>> GetIngrediantsForRecipe(int recipeId)
        {
            return await this.Client.GetAsync<IEnumerable<IngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }

        /// <summary>
        /// Adds or updates an ingrediant of a recipe. If the ingrediant with the given id is not available within the recipe it will be addded. 
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="model">new or updated data</param>
        /// <returns></returns>
        public async Task AddOrUpdateIngrediantOfRecipe(int recipeId, AlterIngrediantViewModel model)
        {
            await this.Client.PutAsJsonAsync(model, $"Recipes/{recipeId}/Ingrediants");
        }

        /// <summary>
        /// Removes an ingrediant from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="ingrediantId">id of an ingrediant</param>
        /// <returns></returns>
        public async Task RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId)
        {
            await this.Client.DeleteAsync($"Recipes/{recipeId}/Ingrediants/{ingrediantId}");
        }
        #endregion

    }
}
