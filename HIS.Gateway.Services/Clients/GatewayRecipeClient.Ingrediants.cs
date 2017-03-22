using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Extensions;
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
        /// Returns the ingrediants of a recipe
        /// </summary>
        /// <param name="recipeId">id of a recipe</param>
        /// <returns></returns>
        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            return await this.Client.GetAsync<IEnumerable<RecipeIngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
