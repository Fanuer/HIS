using System;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.WebApi.Client.Services.Interfaces
{
    internal interface ITagService:IDisposable
    {
        /// <summary>
        /// Get all Tags from the recipe api
        /// </summary>
        /// <param name="searchterm">term to filter the tags</param>
        /// <param name="page">0-based Page for Pagination</param>
        /// <param name="entriesPerPage">Entries per Page. Used for Pagination</param>
        /// <returns></returns>
        Task<ListViewModel<NamedViewModel>> GetAllTagsAsync(string searchterm = "", int page = 0, int entriesPerPage = 10);

        /// <summary>
        /// Creates a new Tags
        /// </summary>
        /// <param name="tagName">Name of the Tag</param>
        /// <returns></returns>
        Task<NamedViewModel> AddTagsAsync(string tagName);

        /// <summary>
        /// Removes an Tags
        /// </summary>
        /// <param name="tagId">ID of an tag</param>
        /// <returns></returns>
        Task DeleteTagAsync(int tagId);

        /// <summary>
        /// Updates an Tag
        /// </summary>
        /// <param name="tagId">Id of the tag to update</param>
        /// <param name="updateModel">new tag data</param>
        /// <returns></returns>
        Task UpdateTagAsync(int tagId, NamedViewModel updateModel);

        /// <summary>
        /// Adds a tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="tagId">Id of the tag</param>
        /// <returns></returns>
        Task AddTagToRecipeAsync(int recipeId, int tagId);

        /// <summary>
        /// Adds a tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of the recipe</param>
        /// <param name="tagName">Name of the tag</param>
        /// <returns></returns>
        Task AddTagToRecipeAsync(int recipeId, string tagName);

        /// <summary>
        /// Removes an tag from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="tagId">id of an tag</param>
        /// <returns></returns>
        Task RemoveTagFromRecipeAsync(int recipeId, int tagId);

        /// <summary>
        /// Removes an tag from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="tagName">id of an tag</param>
        /// <returns></returns>
        Task RemoveTagFromRecipeAsync(int recipeId, string tagName);
    }
}