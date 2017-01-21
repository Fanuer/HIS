using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using HIS.Bot.WebApi.ConfigSections;
using HIS.Bot.WebApi.Extensions;
using HIS.Bot.WebApi.ViewModels;
using HIS.Bot.WebApi.ViewModels.Enum;
using IdentityModel.Client;

namespace HIS.Bot.WebApi.Clients
{
    public class GatewayClient:IDisposable
    {

        #region CONST
        private const string AUTH_COOKIE_NAME = "authCookie";

        #endregion

        #region FIELDS
        private readonly string _apiVersion;
        private readonly HttpClient _client;
        #endregion

        #region CTOR

        public GatewayClient(string apiVersion = "1")
        {
            _apiVersion = apiVersion;
            _client = new HttpClient {BaseAddress = new Uri(this.GetBotData().ClientInfo.GatewayUri)};
        }
        #endregion

        #region METHODS

        public async Task<ViewModels.ListViewModel<ShortRecipeViewModel>> GetRecipes(RecipeSearchViewModel searchModel, int page = 0, int entriesPerPage = 5)
        {
            await this.SetBearerTokenAsync(HttpContext.Current);
            var newUrl = new Uri(this._client.BaseAddress, "Recipes/");
            var query = $"${nameof(page)}={page}&{nameof(entriesPerPage)}={entriesPerPage}";

            if (searchModel != null)
            {
                query  += this._client.ConvertToQueryString(searchModel);
            }

            return await this._client.GetAsync<ViewModels.ListViewModel<ShortRecipeViewModel>>(new Uri(newUrl, query).ToString());
        }

        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            await this.SetBearerTokenAsync(HttpContext.Current);

            return await this._client.GetAsync<IEnumerable<RecipeIngrediantViewModel>>($"Recipes/{recipeId}/Ingrediants");
        }

        public async Task<StepViewModel> GetStepAsync(int recipeId, int stepId = -1, StepDirection direction = StepDirection.ThisStep)
        {
            await this.SetBearerTokenAsync(HttpContext.Current);

            return await this._client.GetAsync<StepViewModel>($"Recipes/{recipeId}/Steps/{stepId}" + (direction != StepDirection.ThisStep ? $"?direction={direction}" : ""));
        }

        public async Task StartCookingAsync(int recipeId)
        {
            await this.SetBearerTokenAsync(HttpContext.Current);
            await this._client.GetAsync($"Recipes/{recipeId}/cooking");
        }


        /// <summary>
        /// Receives a bearer token from the auth service and adds it to this client
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private async Task SetBearerTokenAsync(HttpContext context)
        {
            string bearerToken = "";
            if (context.Request.Cookies.AllKeys.Any(x=>x.Equals(CookieName)))
            {
                bearerToken = context.Request.Cookies.Get(CookieName)?.Value;
            }
            
            if (String.IsNullOrWhiteSpace(bearerToken))
            {
                var botData = GetBotData();

                var disco = await DiscoveryClient.GetAsync(botData.AuthServiceData.AuthServerLocation);
                var tokenClient = new TokenClient(disco.TokenEndpoint, botData.ClientInfo.ClientId, botData.ClientInfo.ClientSecret);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(botData.AuthServiceData.ApiName);

                if (tokenResponse.IsError)
                {
                    var message = "Error on receiving token for this Credential Auth";
                    throw new ArgumentException(message);
                }
                _client.SetBearerToken(tokenResponse.AccessToken);
                var cookie = new HttpCookie(CookieName)
                {
                    HttpOnly = true,
                    Secure = true,
                    Expires = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
                };

                context.Response.Cookies.Add(cookie);

            }
            else
            {
                _client.SetBearerToken(bearerToken);
            }
        }

        private BotData GetBotData()
        {
            var config = System.Configuration.ConfigurationManager.GetSection("botData") as ConfigSections.BotData;
            if (config == null)
            {
                throw new ArgumentNullException(nameof(config), "Configsection 'BotData' must be defined within the web.config");
            }

            return config;
        }

        public static string ConvertToQueryString<T>(T model) where T : class
        {
            const string tempBaseUrl = "http://example.com/";

            if (model == null) { return ""; }

            var builder = new UriBuilder(tempBaseUrl) { Port = -1 };
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                query.Add(propertyInfo.Name, (propertyInfo.GetValue(model) ?? "").ToString());
            }

            builder.Query = query.ToString();
            return builder.ToString().Replace(tempBaseUrl, "");
        }
        #endregion

        #region PROPERTIES
        private string CookieName => $"{AUTH_COOKIE_NAME}_{GetBotData().ClientInfo.ClientId.Replace("_", "-")}";

        public void Dispose()
        {
            _client?.Dispose();
        }

        #endregion

    }
}