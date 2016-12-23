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
    public class RecipeClient:S2SClientBase, IRecipeBotClient
    {
        #region CONST
        #endregion

        #region FIELDS
        
        #endregion

        #region CTOR        
        public RecipeClient(IOptions<AuthServerInfoOptions> authOptions, IOptions<GatewayClientInfoOptions> clientOptions, ILoggerFactory factory) 
            : base(authOptions, clientOptions, factory.CreateLogger<RecipeClient>())
        {
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

        public Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            throw new NotImplementedException();
        }

        public Task<StepViewModel> GetStepAsyc(int recipeId, int stepId, StepDirection direction = StepDirection.ThisStep)
        {
            throw new NotImplementedException();
        }

        public Task StartCookingAsync()
        {
            throw new NotImplementedException();
        }
    }
}
