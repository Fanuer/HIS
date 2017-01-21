using System;
using System.Net.Http;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Extensions;
using HIS.Helpers.Options;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace HIS.Helpers.Web.Clients
{
    public abstract class S2SClientBase:IDisposable
    {
        #region CONST

        private const string AUTH_COOKIE_NAME = "authCookie";
        #endregion

        #region FIELDS
        protected readonly ClientInformation _clientOptions;
        private readonly HttpClient _client;
        #endregion

        #region CTOR

        protected S2SClientBase(IOptions<ClientInformation> clientOptions, ILogger logger)
        {
            if (clientOptions?.Value == null) { throw new ArgumentException(nameof(clientOptions)); }
            if (clientOptions?.Value.Credentials == null) { throw new ArgumentException(nameof(clientOptions.Value.Credentials)); }
            if (String.IsNullOrWhiteSpace(clientOptions.Value.Credentials.ClientId)){ throw new ArgumentNullException(nameof(clientOptions.Value.Credentials.ClientId)); }
            if (String.IsNullOrWhiteSpace(clientOptions.Value.Credentials.ClientSecret)){ throw new ArgumentNullException(nameof(clientOptions.Value.Credentials.ClientSecret)); }
            if (String.IsNullOrWhiteSpace(clientOptions.Value.TargetApiName)){ throw new ArgumentNullException(nameof(clientOptions.Value.TargetApiName)); }
            if (String.IsNullOrWhiteSpace(clientOptions.Value.TargetBaseUrl)){ throw new ArgumentNullException(nameof(clientOptions.Value.TargetBaseUrl)); }
            
            if (logger == null){ throw new ArgumentNullException(nameof(logger)); }

            this._client = new HttpClient {BaseAddress = new Uri(clientOptions.Value.TargetBaseUrl)};

            this.ApiName = clientOptions.Value.TargetApiName;
            _clientOptions = clientOptions.Value;
            this.Logger = logger;
        }

        protected S2SClientBase(IOptions<GatewayInformation> gatewayInformation, ILogger logger)
        {
            if (gatewayInformation?.Value == null) { throw new ArgumentException(nameof(gatewayInformation)); }
            if (gatewayInformation?.Value.Credentials == null) { throw new ArgumentException(nameof(gatewayInformation.Value.Credentials)); }
            if (String.IsNullOrWhiteSpace(gatewayInformation.Value.Credentials.ClientId)) { throw new ArgumentNullException(nameof(gatewayInformation.Value.Credentials.ClientId)); }
            if (String.IsNullOrWhiteSpace(gatewayInformation.Value.Credentials.ClientSecret)) { throw new ArgumentNullException(nameof(gatewayInformation.Value.Credentials.ClientSecret)); }

            if (logger == null) { throw new ArgumentNullException(nameof(logger)); }
            
            this.Logger = logger;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Adds a Access Token to the Httpthis
        /// Stores a recieved token 
        /// </summary>
        /// <param name="context">current HTTPContext </param>
        /// <returns></returns>
        public async Task SetBearerTokenAsync(HttpContext context = null)
        {
            string bearerToken = null;

            context?.Request.Cookies.TryGetValue(CookieName, out bearerToken);

            if (String.IsNullOrWhiteSpace(bearerToken))
            {
                var disco = await DiscoveryClient.GetAsync(this._clientOptions.AuthServerLocation);
                var tokenClient = new TokenClient(disco.TokenEndpoint, this._clientOptions.Credentials.ClientId, this._clientOptions.Credentials.ClientSecret);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(this.ApiName);

                if (tokenResponse.IsError)
                {
                    var message = "Error on receiving token for this Credential Auth";
                    Logger.LogError(message +": " + tokenResponse.Error);
                    throw new ArgumentException(message);
                }
                this._client.SetBearerToken(tokenResponse.AccessToken);
                context?.Response.Cookies.Append(CookieName, tokenResponse.AccessToken, new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn),
                    HttpOnly = true,
                    Secure = true
                });

                Logger.LogInformation($"Added new Access Token for Api {this._clientOptions.TargetApiName}. Expires at {DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)}");
            }
            else
            {
                Logger.LogInformation($"Used cached Access Token for Api {this._clientOptions.TargetApiName}");
                this._client.SetBearerToken(bearerToken);
            }
        }
        public void Dispose()
        {
            this._client?.Dispose();
        }

        public void SetBearerToken(string bearerToken)
        {
            this.Client.SetBearerToken(bearerToken);
        }
        #endregion

        #region PROPERTIES
        public ILogger Logger { get; }
        public string ApiName { get; }

        private string CookieName => $"{AUTH_COOKIE_NAME}_{this._clientOptions.Credentials.ClientId.Replace("_", "-")}";

        protected HttpClient Client => _client;

        #endregion

    }
}
