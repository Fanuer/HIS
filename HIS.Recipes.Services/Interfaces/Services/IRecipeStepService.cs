using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.Services.Interfaces.Services
{
    public interface IRecipeStepService: IDisposable
    {
        /// <summary>
        /// Returns a steps of a given recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <returns></returns>
        IQueryable<StepViewModel> GetStepsForRecipe(int recipeId);

        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        Task UpdateAsync(int id, StepUpdateViewModel model);

        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        Task<StepViewModel> AddAsync(int recipeId, StepCreateViewModel creationModel);
        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        Task RemoveAsync(int id);

        /// <summary>
        /// Overwrites all steps of a recipe by the given data
        /// </summary>
        /// <param name="recipeId">id of the owing recipe</param>
        /// <param name="model">new steps</param>
        /// <returns></returns>
        Task UpdateAllStepsAsync(int recipeId, ICollection<string> model);
    }
}
