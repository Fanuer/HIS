using System.Threading.Tasks;
using HIS.Helpers.Extensions;
using HIS.Helpers.Options;
using HIS.Helpers.Web.Clients;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.WebApi.Client.Services.Interfaces;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Recipes.WebApi.Client.Services
{
    internal class RecipeImageService : S2SClientBase, IRecipeImageService
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public RecipeImageService(IOptions<ClientInformation> clientOptions, ILoggerFactory factory)
            : base(clientOptions.Value, factory.CreateLogger<RecipeImageService>())
        {
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Gets all Images of a Recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public async Task<ListViewModel<RecipeImageViewModel>> GetRecipeImages(int recipeId)
        {
            return await this.Client.GetAsync<ListViewModel<RecipeImageViewModel>>($"Recipes/{recipeId}/Images");
        }

        /// <summary>
        /// Create a new Recipe image
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="newFile">Image file</param>
        /// <returns></returns>
        public async Task<RecipeImageViewModel> AddRecipeImageAsync(int recipeId, IFileInfo newFile)
        {
            return await this.Client.PostAsJsonReturnAsync<IFileInfo, RecipeImageViewModel>(newFile, $"Recipes/{recipeId}/Images");
        }

        /// <summary>
        /// Removes an existing image
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="imageId">Id of an Image</param>
        /// <returns></returns>
        public async Task DeleteRecipeImageAsync(int recipeId, int imageId)
        {
            await this.Client.DeleteAsync($"Recipes/{recipeId}/Images/{imageId}");
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
