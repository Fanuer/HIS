using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.Services.Interfaces.Services
{
    public interface ITagService:IDisposable
    {
        /// <summary>
        /// Get all Tags
        /// </summary>
        /// <returns></returns>
        IQueryable<NamedViewModel> GetTags();
        
        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        Task UpdateAsync(int id, NamedViewModel model);
        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        Task<NamedViewModel> AddAsync(string creationModel);
        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        Task RemoveAsync(int id);

        /// <summary>
        /// Adds a Tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="tagName">tag name</param>
        /// <returns></returns>
        Task AddTagToRecipeAsync(int recipeId, string tagName);

        /// <summary>
        /// Adds a Tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="tagId">Id of a tag entry</param>
        /// <returns></returns>
        Task AddTagToRecipeAsync(int recipeId, int tagId);


        /// <summary>
        /// Removes a Tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="tagId">Id of a tag entry</param>
        /// <returns></returns>
        Task RemoveTagFromRecipeAsync(int recipeId, int tagId);


        /// <summary>
        /// Removes a Tag to a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="tagName">tag name</param>
        /// <returns></returns>
        Task RemoveTagFromRecipeAsync(int recipeId, string tagName);
    }
}
