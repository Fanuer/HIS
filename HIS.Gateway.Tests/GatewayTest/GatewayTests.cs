using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Options;
using HIS.Helpers.Test;
using HIS.Recipes.Models.ViewModels;
using Microsoft.Extensions.Options;
using Xunit;

namespace HIS.Gateway.Tests.GatewayTest
{
    public class GatewayTests:TestBase
    {
        [Fact]
        public async Task GetRecipes()
        {
            using (var client = await this.CreateExternalClient())
            {
                const int entriesPerPage = 5;
                try
                {
                    var recipes = await client.GetRecipes(entriesPerPage: entriesPerPage);

                    Assert.NotNull(recipes?.Entries);
                    Assert.NotEmpty(recipes.Entries);
                    Assert.Equal(entriesPerPage, recipes.Entries.Count());
                }
                catch (ServerException e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
        }

        [Fact]
        public async Task GetRecipeIngrediants()
        {
            using (var client = await this.CreateExternalClient())
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
            using (var client = await this.CreateExternalClient())
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

        [Fact]
        public async Task SearchForRecipesByName()
        {
            using (var client = await this.CreateExternalClient())
            {
                try
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
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
                
            }
        }

        [Fact]
        public async Task SearchForRecipesByTag()
        {
            using (var client = await this.CreateExternalClient())
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
                Assert.True(result.Entries.Any(x => x.Id.Equals(firstTagRecipe.Id)));
            }
        }


        [Fact]
        public async Task SearchForRecipesByIngrediant()
        {
            using (var client = await this.CreateExternalClient())
            {
                var recipe = await client.GetRecipes();
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

        /// <summary>
        /// Creates a Bot client that connects itself to the api gateway
        /// </summary>
        /// <returns></returns>
        private async Task<GatewayClient> CreateExternalClient()
        {
            var config = GetConfig();
            var info = new GatewayInformation(config);


            var clientInfo = new ClientInformation()
            {
                Credentials = new ClientCredentials()
                {
                    ClientId = info.ExternalClientInfo.ClientId,
                    ClientSecret = info.ExternalClientInfo.ClientSecret
                },
                TargetApiName = info.GatewayApiName,
                TargetBaseUrl = info.GatewayApiBaseUrl,
                AuthServerLocation = info.AuthServerUrl
            };
            var clientInfoOptions = new OptionsWrapper<ClientInformation>(clientInfo);

            var client = new GatewayClient(clientInfoOptions, new MockLoggerFactory<object>());
            var tokenResponse = await GetAccessTokenFromAuthServer(clientInfo);
            client.SetBearerToken(tokenResponse.AccessToken);

            return client;
        }

        
    }
}
