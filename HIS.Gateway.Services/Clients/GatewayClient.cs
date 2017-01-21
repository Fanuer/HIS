using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Web.Clients;
using HIS.Helpers.Options;
using HIS.Helpers.Extensions;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Gateway.Services.Clients
{
    /// <summary>
    /// Represents a external client that calls the gateway
    /// </summary>
    public class GatewayClient:S2SClientBase
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public GatewayClient(IOptions<ClientInformation> clientOptions, ILoggerFactory loggerFactory)
               : base(clientOptions, loggerFactory.CreateLogger<GatewayClient>())
        {
        }

        #endregion

        #region METHODS
        public async Task<ListViewModel<ShortRecipeViewModel>> GetRecipes(RecipeSearchViewModel searchModel = null, int page = 0, int entriesPerPage = 10)
        {
            var newUrl = new Uri(this.Client.BaseAddress, "Recipes/");
            var query = "";

            if (searchModel != null)
            {
                var searchQuery = this.Client.AddToUrlAsQueryString(searchModel);
                if (!String.IsNullOrWhiteSpace(searchQuery))
                {
                    query = $"?{searchQuery}";
                }
            }

            query = QueryHelpers.AddQueryString(query, nameof(page), page.ToString());
            query = QueryHelpers.AddQueryString(query, nameof(entriesPerPage), entriesPerPage.ToString());

            return await this.Client.GetAsync<ListViewModel<ShortRecipeViewModel>>(new Uri(newUrl, query).ToString());
        }

        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            return await this.Client.GetAsync<IEnumerable<RecipeIngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }

        public async Task<StepViewModel> GetStepAsync(int recipeId, int stepId = -1, StepDirection direction = StepDirection.ThisStep)
        {
            return await this.Client.GetAsync<StepViewModel>($"Recipes/{recipeId}/Steps/{stepId}" + (direction != StepDirection.ThisStep ? $"?direction={direction}" : ""));
        }

        public async Task StartCookingAsync(int recipeId)
        {
            await this.Client.PostAsync($"Recipes/{recipeId}/cooking");
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}
