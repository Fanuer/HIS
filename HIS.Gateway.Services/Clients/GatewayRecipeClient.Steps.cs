using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Extensions;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Gateway.Services.Clients
{
    internal partial class GatewayRecipeClient
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        /// <summary>
        /// Gets a recipe step
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <param name="stepId">id of the current step. If not set, the first step will be return</param>
        /// <param name="direction">navigation direction</param>
        /// <returns></returns>
        public async Task<StepViewModel> GetStepAsync(int recipeId, int stepId = -1, StepDirection direction = StepDirection.ThisStep)
        {
            return await this.Client.GetAsync<StepViewModel>($"Recipes/{recipeId}/Steps/{stepId}" + (direction != StepDirection.ThisStep ? $"?direction={direction}" : ""));
        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
