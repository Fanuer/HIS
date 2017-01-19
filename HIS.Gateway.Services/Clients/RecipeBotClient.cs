using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Options;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HIS.Helpers.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace HIS.Gateway.Services.Clients
{
    internal class RecipeBotClient:S2SClientBase, IRecipeBotClient
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR        
        public RecipeBotClient(IOptions<AuthServerInfoOptions> authOptions, IOptions<GatewayClientInfoOptions> clientOptions, ILoggerFactory factory) 
            : base(authOptions, clientOptions, factory.CreateLogger<RecipeBotClient>(), "recipe-api")
        {
        }

        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        #endregion

        public async Task<ListViewModel<ShortRecipeViewModel>> GetRecipes(RecipeSearchViewModel searchModel, int page = 0, int entriesPerPage = 10)
        {
            var newUrl = new Uri(this.BaseAddress, "Recipes/");
            var url = this.AddToUrlAsQueryString(newUrl.ToString(), nameof(searchModel), searchModel);
            url = QueryHelpers.AddQueryString(url, nameof(page), page.ToString());
            url = QueryHelpers.AddQueryString(url, nameof(entriesPerPage), entriesPerPage.ToString());
            return await this.GetAsync<ListViewModel<ShortRecipeViewModel>>(url);
        }

        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            return await this.GetAsync<IEnumerable<RecipeIngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }

        public async Task<StepViewModel> GetStepAsync(int recipeId, int stepId = -1, StepDirection direction = StepDirection.ThisStep)
        {
            return await this.GetAsync<StepViewModel>($"Recipes/{recipeId}/Steps/{stepId}" + (direction != StepDirection.ThisStep ? $"?direction={direction}" : ""));
        }

        public async Task StartCookingAsync(int recipeId)
        {
            await this.GetAsync<StepViewModel>($"Recipes/{recipeId}/cooking");
        }
    }
}
