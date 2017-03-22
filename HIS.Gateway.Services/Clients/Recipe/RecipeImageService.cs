using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients.Recipe.Interfaces;
using HIS.Helpers.Options;
using HIS.Recipes.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HIS.Helpers.Extensions;
using Microsoft.Extensions.FileProviders;

namespace HIS.Gateway.Services.Clients.Recipe
{
    internal class RecipeImageService : RmServiceBase, IRecipeImageService
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public RecipeImageService(IOptions<GatewayInformation> clientOptions, ILoggerFactory factory)
    : base(clientOptions, factory.CreateLogger<RecipeImageService>())
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
