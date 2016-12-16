using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace HIS.Recipes.Services.Interfaces.Services
{
    /// <summary>
    /// Access to recipe images
    /// </summary>
    public interface IRecipeImageService : IDisposable
    {
        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="data">New image data</param>
        /// <returns></returns>
        Task UpdateAsync(int id, int recipeId, IFormFile data);
        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        Task<RecipeImageViewModel> AddAsync(int recipeId, IFormFile creationModel);
        /// <summary>
        /// Get all recipe images
        /// </summary>
        /// <param name="recipeId">id if the recipe</param>
        /// <returns></returns>
        IQueryable<RecipeImageViewModel> GetImages(int recipeId);
        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        Task RemoveAsync(int id);

        /// <summary>
        /// Returns an image 
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        Task<RecipeImageViewModel> GetImage(int imageId);
    }
}
