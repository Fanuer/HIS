using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using Microsoft.EntityFrameworkCore.Design;

namespace HIS.Recipes.Services.Interfaces.Services
{
    /// <summary>
    /// Grants access to ingrediant data
    /// </summary>
    public interface IIngrediantService:IDisposable
    {
        /// <summary>
        /// Returns a list of all ingrediants
        /// </summary>
        /// <returns></returns>
        IQueryable<IngrediantStatisticViewModel> GetIngrediantList();
        /// <summary>
        /// Returns a list 
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        IQueryable<IngrediantViewModel> GetIngrediantsForRecipe(int recipeId);
        /// <summary>
        /// Adds an ingrediant
        /// </summary>
        /// <param name="name">name of the ingrediant</param>
        /// <returns></returns>
        Task<NamedViewModel> AddAsync(string name);
        /// <summary>
        /// Changes an existing ingrediant
        /// </summary>
        /// <param name="id">id of the updating ingrediant</param>
        /// <param name="model">new data</param>
        /// <returns></returns>
        Task UpdateAsync(int id, NamedViewModel model);
        /// <summary>
        /// Removes an existing ingrediant
        /// </summary>
        /// <param name="id">ingrediantId</param>
        /// <returns></returns>
        Task RemoveAsync(int id);
        /// <summary>
        /// Adds or updates an ingrediant on a recipe
        /// </summary>
        /// <param name="model">data to a recipe ingrediant</param>
        /// <returns></returns>
        Task AddOrUpdateIngrediantToRecipeAsync(AlterIngrediantViewModel model);
        /// <summary>
        /// Removes an ingrediant from a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="ingrediantId">id of an ingrediant</param>
        /// <returns></returns>
        Task RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId);

    }
}
