using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Recipes.WebApi.Client.Services.Interfaces
{
    internal interface IStepService:IDisposable
    {
        /// <summary>
        /// Get all Steps of a recipe 
        /// </summary>
        /// <param name="recipeId">term to filter the steps</param>
        /// <returns></returns>
        Task<IEnumerable<StepViewModel>> GetAllRecipeStepsAsync(int recipeId);

        /// <summary>
        /// Creates a new Steps for a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="newStep">Data of the new Step</param>
        /// <returns></returns>
        Task<StepViewModel> AddStepsAsync(int recipeId, StepCreateViewModel newStep);

        /// <summary>
        /// Removes a Step from a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="stepId">Id of an step</param>
        /// <returns></returns>
        Task DeleteStepAsync(int recipeId, int stepId);

        /// <summary>
        /// Updates all Steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="newSteps">List of Steps. Will override the old ones. Array order will be step order</param>
        /// <returns></returns>
        Task UpdateStepAsync(int recipeId, IEnumerable<string> newSteps);

        /// <summary>
        /// Updates a Step within the recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="stepId">Id of the step to update</param>
        /// <param name="updateModel">new step data</param>
        /// <returns></returns>
        Task UpdateStepAsync(int recipeId, int stepId, StepViewModel updateModel);
    }
}