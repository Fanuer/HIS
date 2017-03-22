using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients.Recipe.Interfaces;
using HIS.Helpers.Options;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HIS.Helpers.Extensions;

namespace HIS.Gateway.Services.Clients.Recipe
{
    internal class TagService : RmServiceBase, ITagService
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public TagService(IOptions<GatewayInformation> clientOptions, ILoggerFactory loggerFactory)
    : base(clientOptions, loggerFactory?.CreateLogger<TagService>())
        {
        }

        #endregion

        #region METHODS
        #region General CRUD

        /// <summary>
        /// Get all tags from the recipe api
        /// </summary>
        /// <param name="searchterm">term to filter the tags</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        public async Task<ListViewModel<NamedViewModel>> GetAllTagsAsync(string searchterm = "", int page = 0, int entriesPerPage = 10)
        {
            var uri = this.Client.AddToQueryString("Tags", new { searchterm, page, entriesPerPage });
            return await this.Client.GetAsync<ListViewModel<NamedViewModel>>(uri);
        }

        /// <summary>
        /// Creates a new tags
        /// </summary>
        /// <param name="tagName">Name of the Tag</param>
        /// <returns></returns>
        public async Task<NamedViewModel> AddTagsAsync(string tagName)
        {
            return await this.Client.PostAsJsonReturnAsync<string, NamedViewModel>(tagName, "Tags");
        }

        /// <summary>
        /// Removes a tag
        /// </summary>
        /// <param name="tagId">ID of an tag</param>
        /// <returns></returns>
        public async Task DeleteTagAsync(int tagId)
        {
            await this.Client.DeleteAsync($"Tag/{tagId}");
        }

        /// <summary>
        /// Updates a tag
        /// </summary>
        /// <param name="tagId">Id of the tag to update</param>
        /// <param name="updateModel">new tag data</param>
        /// <returns></returns>
        public async Task UpdateTagAsync(int tagId, NamedViewModel updateModel)
        {
            await this.Client.PutAsJsonAsync(updateModel, $"Tag/{tagId}");
        }

        #endregion

        #region Recipe specific

        /// <summary>
        /// Adds a tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="tagId">Id of the tag</param>
        /// <returns></returns>
        public async Task AddTagToRecipeAsync(int recipeId, int tagId)
        {
            await this.Client.PostAsync($"Recipes/{recipeId}/Tags/{tagId}");
        }

        /// <summary>
        /// Adds a tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="tagName">Name of the tag</param>
        /// <returns></returns>
        public async Task AddTagToRecipeAsync(int recipeId, string tagName)
        {
            await this.Client.PostAsync($"Recipes/{recipeId}/Tags/{tagName}");
        }
        /// <summary>
        /// Removes an tag from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="tagId">id of an tag</param>
        /// <returns></returns>
        public async Task RemoveTagFromRecipeAsync(int recipeId, int tagId)
        {
            await this.Client.DeleteAsync($"Recipes/{recipeId}/Tags/{tagId}");
        }

        /// <summary>
        /// Removes an tag from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="tagName">id of an tag</param>
        /// <returns></returns>
        public async Task RemoveTagFromRecipeAsync(int recipeId, string tagName)
        {
            await this.Client.DeleteAsync($"Recipes/{recipeId}/Tags/{tagName}");
        }
        #endregion

        #endregion

        #region PROPERTIES
        #endregion
    }
}
