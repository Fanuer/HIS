using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients;
using HIS.Helpers.Options;
using HIS.Helpers.Test;
using HIS.Recipes.Models.ViewModels;
using Microsoft.Extensions.Options;
using Xunit;

namespace HIS.Gateway.Tests.GatewayClientTests
{
    /// <summary>
    /// These tests test the recipeApi as consumed by a gateway-client
    /// </summary>
    public class RecipeApiTests: TestBase
    {
        private const string APINAME_RECIPEAPI = "recipe-api";

        [Fact]
        public async Task Test_GetAccessToken()
        {
            var config = GetConfig();
            var info = new GatewayInformation(config);

            var clientInfo = new ClientInformation()
            {
                Credentials = new ClientCredentials()
                {
                    ClientId = info.ClientInfo.ClientId,
                    ClientSecret = info.ClientInfo.ClientSecret
                },
                TargetBaseUrl = info.Apis[APINAME_RECIPEAPI],
                TargetApiName = APINAME_RECIPEAPI,
                AuthServerLocation = info.AuthServerUrl
            };

            var tokenResponse = await GetAccessTokenFromAuthServer(clientInfo);

            Assert.False(tokenResponse.IsError);
            Assert.NotEmpty(tokenResponse.AccessToken);
        }

        [Fact]
        public async Task GetRecipes()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                var recipe = await client.GetRecipes();
                Assert.NotNull(recipe);
                Assert.NotNull(recipe.Entries);
                Assert.NotEmpty(recipe.Entries);
            }
        }

        [Fact]
        public async Task SearchForRecipesByName()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                var allRecipe = await client.GetRecipes();
                var searchModel = new RecipeSearchViewModel()
                {
                    Name = allRecipe.Entries.First().Name
                };

                var result = await client.GetRecipes(searchModel);
                Assert.NotNull(result);
                Assert.NotNull(result.Entries);
                Assert.NotEmpty(result.Entries);
                Assert.Equal(1, result.Entries.Count());

                var firstResult = result.Entries.First();
                var compareRecipe = result.Entries.First();

                Assert.Equal(compareRecipe.Name, firstResult.Name);
                Assert.Equal(compareRecipe.Creator, firstResult.Creator);
                Assert.Equal(compareRecipe.Id, firstResult.Id);
            }
        }

        [Fact]
        public async Task SearchForRecipesByTag()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                var recipe = await client.GetRecipes();
                var firstTagRecipe = recipe.Entries.First(x => x.Tags.Any());
                var searchModel = new RecipeSearchViewModel()
                {
                    Tags = new List<string>() { firstTagRecipe.Tags.First() }
                };

                var result = await client.GetRecipes(searchModel);

                Assert.NotNull(result);
                Assert.NotNull(result.Entries);
                Assert.NotEmpty(result.Entries);
                Assert.True(result.Entries.Any(x=>x.Id.Equals(firstTagRecipe.Id)));
            }
        }


        [Fact]
        public async Task SearchForRecipesByIngrediant()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                var recipe = await client.GetRecipes(entriesPerPage: Int32.MaxValue);
                var firstRecipe = recipe.Entries.First();
                var ingrediants = await client.GetRecipeIngrediantsAsync(firstRecipe.Id);
                var searchModel = new RecipeSearchViewModel()
                {
                    Ingrediants = new List<string>() { ingrediants.First().Name }
                };

                var result = await client.GetRecipes(searchModel);

                Assert.NotNull(result);
                Assert.NotNull(result.Entries);
                Assert.NotEmpty(result.Entries);
                Assert.True(result.Entries.Any(x => x.Id.Equals(firstRecipe.Id)));
            }
        }

        [Fact]
        public async Task GetRecipeIngrediants()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                var recipes = await client.GetRecipes();
                if (recipes.Entries.Any())
                {
                    var ingrediants = await client.GetRecipeIngrediantsAsync(recipes.Entries.First().Id);
                    Assert.NotNull(ingrediants);
                    Assert.NotEmpty(ingrediants);
                }
            }
        }

        [Fact]
        public async Task GetRecipeStep()
        {
            using (var client = await this.CreateRecipeApiClient())
            {
                try
                {
                    var recipes = await client.GetRecipes();
                    if (recipes.Entries.Any())
                    {
                        var firstRecipe = recipes.Entries.First();
                        var step = await client.GetStepAsync(firstRecipe.Id);
                        Assert.NotNull(step);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    
                    throw;
                }
                
            }
        }

        /// <summary>
        /// Creates a gateway-client that connects itself to the recipe-api
        /// </summary>
        /// <returns></returns>
        private async Task<GatewayRecipeClient> CreateRecipeApiClient()
        {
            var config = GetConfig();
            var info = new GatewayInformation(config);
            
            var clientInfo = new Helpers.Options.GatewayInformation()
            {
                Credentials = new ClientCredentials()
                {
                    ClientId = info.ClientInfo.ClientId,
                    ClientSecret = info.ClientInfo.ClientSecret,
                },
                AuthServerLocation = info.AuthServerUrl,
                GatewayClients = info.Apis
            };
            var clientInfoOptions = new OptionsWrapper<Helpers.Options.GatewayInformation>(clientInfo);
            

            var client = new GatewayRecipeClient(clientInfoOptions, new MockLoggerFactory<object>());
            var tokenResponse = await GetAccessTokenFromAuthServer(clientInfo.GetClientInformation(APINAME_RECIPEAPI));
            client.SetBearerToken(tokenResponse.AccessToken);

            return client;
        }
    }
}
