using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.Services.Interfaces.Services
{
    internal interface IRecipeStepService: IDisposable
    {
        /// <summary>
        /// Returns a steps of a given recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <returns></returns>
        IQueryable<StepViewModel> GetStepsForRecipe(Guid recipeId);

        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="model">New Data</param>
        /// <returns></returns>
        Task UpdateAsync(Guid id, StepViewModel model);
        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        Task<StepViewModel> AddAsync(StepCreateViewModel creationModel);
        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        Task RemoveAsync(Guid id);
    }
}
