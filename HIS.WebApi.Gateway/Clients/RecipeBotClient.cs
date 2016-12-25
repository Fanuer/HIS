using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HIS.Helpers.Options;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.WebApi.Gateway.Interfaces;
using HIS.WebApi.Gateway.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.WebApi.Gateway.Clients
{
    public class RecipeBotClient:S2SClientBase, IRecipeBotClient
    {
        #region CONST
        #endregion

        #region FIELDS
        
        #endregion

        #region CTOR        
        public RecipeBotClient(IOptions<AuthServerInfoOptions> authOptions, IOptions<GatewayClientInfoOptions> clientOptions, ILoggerFactory factory) 
            : base(authOptions, clientOptions, factory.CreateLogger<RecipeBotClient>())
        {
            if (String.IsNullOrWhiteSpace(clientOptions?.Value?.BaseUri)){ throw new ArgumentNullException(nameof(clientOptions.Value.BaseUri)); }
#warning muss noch in der config gesetzt werden
            this.BaseAddress = new Uri(clientOptions.Value.BaseUri);
        }

        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        #endregion

        public async Task<IEnumerable<ShortRecipeViewModel>> GetRecipes()
        {
            return await this.GetAsync<IEnumerable<ShortRecipeViewModel>>("Recipes/");
        }

        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            return await this.GetAsync<IEnumerable<RecipeIngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }

        public async Task<StepViewModel> GetStepAsyc(int recipeId, int stepId, StepDirection direction = StepDirection.ThisStep)
        {
            return await this.GetAsync<StepViewModel>($"Recipes/{recipeId}/Steps/{stepId}" + (direction != StepDirection.ThisStep ? $"?direction={direction}" : ""));
        }

        public async Task StartCookingAsync(int recipeId)
        {
            await this.GetAsync<StepViewModel>($"Recipes/{recipeId}/cooking");
        }
    }
}
