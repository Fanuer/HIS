using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients;
using HIS.Helpers.Options;
using HIS.Helpers.Test;
using HIS.Recipes.Models.ViewModels;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Xunit;

namespace HIS.Gateway.Tests.ClientTests
{
    public class RecipeApiTests
    {
        private const string APINAME = "recipe-api";

        [Fact]
        public async Task Test_GetAccessToken()
        {
            var config = GetConfig();
            var gatewaysInfos = new GatewayInformation(config);

            var tokenResponse = await GetAccessTokenFromAuthServer(gatewaysInfos);

            Assert.False(tokenResponse.IsError);
            Assert.NotEmpty(tokenResponse.AccessToken);
        }

        [Fact]
        public async Task GetRecipes()
        {
            using (var client = await this.CreateInternalClient())
            {
                var recipe = await client.GetRecipes();
                Assert.NotNull(recipe);
            }
        }

        [Fact]
        public async Task GetRecipeIngrediants()
        {
            using (var client = await this.CreateInternalClient())
            {
                var recipes = await client.GetRecipes();
                var shortRecipeViewModels = recipes as ShortRecipeViewModel[] ?? recipes.ToArray();
                if (shortRecipeViewModels.Any())
                {
                    var ingrediants = await client.GetRecipeIngrediantsAsync(shortRecipeViewModels.First().Id);
                    Assert.NotNull(ingrediants);
                    Assert.NotEmpty(ingrediants);
                }
            }
        }

        [Fact]
        public async Task GetRecipeStep()
        {
            using (var client = await this.CreateInternalClient())
            {
                var recipes = await client.GetRecipes();
                var shortRecipeViewModels = recipes as ShortRecipeViewModel[] ?? recipes.ToArray();
                if (shortRecipeViewModels.Any())
                {
                    var firstRecipe = shortRecipeViewModels.First();
                    var step = await client.GetStepAsync(firstRecipe.Id);
                    Assert.NotNull(step);
                }
            }
        }

        private async Task<TokenResponse> GetAccessTokenFromAuthServer(GatewayInformation info)
        {
            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(info.AuthServerUrl);

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, info.ClientInfo.ClientId, info.ClientInfo.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(APINAME);
            return tokenResponse;
        }
        private IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder()
                                    .AddJsonFile("appsettings.keys.json", optional: true, reloadOnChange: true)
                                    .AddEnvironmentVariables();
            return builder.Build();
        }

        /// <summary>
        /// Creates a Client, the Gateway is using to access recipe-data
        /// </summary>
        /// <returns></returns>
        private async Task<RecipeBotClient> CreateInternalClient()
        {
            var config = GetConfig();
            var info = new GatewayInformation(config);
            
            var authServerInfos = new AuthServerInfoOptions() {ApiName = info.GatewayApiName, AuthServerLocation = info.AuthServerUrl, UseHttps = false};
            var authServerOptions = new OptionsWrapper<AuthServerInfoOptions>(authServerInfos);

            var clientInfo = new GatewayClientInfoOptions()
            {
                ClientId = info.ClientInfo.ClientId,
                ClientSecret = info.ClientInfo.ClientSecret,
                GatewayClients = info.Apis
            };
            var clientInfoOptions = new OptionsWrapper<GatewayClientInfoOptions>(clientInfo);

            var client = new RecipeBotClient(authServerOptions, clientInfoOptions, new MockLoggerFactory<object>());
            var tokenResponse = await GetAccessTokenFromAuthServer(info);
            client.SetBearerToken(tokenResponse.AccessToken);

            return client;
        }

    }
}
