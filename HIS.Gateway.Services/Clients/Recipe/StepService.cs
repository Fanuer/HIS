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
    internal class StepService : RmServiceBase, IStepService
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public StepService(IOptions<GatewayInformation> clientOptions, ILoggerFactory loggerFactory)
    : base(clientOptions, loggerFactory?.CreateLogger<StepService>())
        {
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Get all Steps of a recipe 
        /// </summary>
        /// <param name="recipeId">term to filter the steps</param>
        /// <returns></returns>
        public async Task<IEnumerable<StepViewModel>> GetAllRecipeStepsAsync(int recipeId)
        {
            return await this.Client.GetAsync<IEnumerable<StepViewModel>>($"Recipes/{recipeId}/Steps");
        }

        /// <summary>
        /// Creates a new Steps for a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="newStep">Data of the new Step</param>
        /// <returns></returns>
        public async Task<StepViewModel> AddStepsAsync(int recipeId, StepCreateViewModel newStep)
        {
            return await this.Client.PostAsJsonReturnAsync<StepCreateViewModel, StepViewModel>(newStep, $"Recipes/{recipeId}/Steps");
        }

        /// <summary>
        /// Removes a Step from a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="stepId">Id of an step</param>
        /// <returns></returns>
        public async Task DeleteStepAsync(int recipeId, int stepId)
        {
            await this.Client.DeleteAsync($"Recipes/{recipeId}/Steps/{stepId}");
        }

        /// <summary>
        /// Updates all Steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="newSteps">List of Steps. Will override the old ones. Array order will be step order</param>
        /// <returns></returns>
        public async Task UpdateStepAsync(int recipeId, IEnumerable<string> newSteps)
        {
            await this.Client.PutAsJsonReturnAsync<IEnumerable<string>, IEnumerable<StepViewModel>>(newSteps, $"Recipes/{recipeId}/Steps");
        }

        /// <summary>
        /// Updates a Step within the recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="stepId">Id of the step to update</param>
        /// <param name="updateModel">new step data</param>
        /// <returns></returns>
        public async Task UpdateStepAsync(int recipeId, int stepId, StepViewModel updateModel)
        {
            await this.Client.PutAsJsonAsync(updateModel, $"Recipes/{recipeId}/Steps/{stepId}");
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
