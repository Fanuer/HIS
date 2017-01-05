using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Options;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

        public async Task<IEnumerable<ShortRecipeViewModel>> GetRecipes()
        {
            return await this.GetAsync<IEnumerable<ShortRecipeViewModel>>("Recipes/");
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
