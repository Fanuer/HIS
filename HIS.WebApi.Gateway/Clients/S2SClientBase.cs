using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using HIS.Helpers.Extensions;
using HIS.Helpers.Options;
using IdentityModel.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Remotion.Linq.Parsing.Structure.IntermediateModel;

namespace HIS.WebApi.Gateway.Clients
{
    public abstract class S2SClientBase:HttpClient
    {
        #region CONST

        private const string AUTH_COOKIE_NAME = "authCookie";
        #endregion

        #region FIELDS
        private readonly AuthServerInfoOptions _authOptions;
        private readonly ClientInfoOptions _clientOptions;
        #endregion

        #region CTOR

        protected S2SClientBase(IOptions<AuthServerInfoOptions> authOptions, IOptions<ClientInfoOptions> thisOptions, ILogger logger)
        {
            if (authOptions?.Value == null){ throw new ArgumentNullException(nameof(authOptions)); }
            if (String.IsNullOrWhiteSpace(authOptions.Value.ApiName)){ throw new ArgumentNullException(nameof(authOptions.Value.ApiName)); }
            if (String.IsNullOrWhiteSpace(authOptions.Value.AuthServerLocation)){ throw new ArgumentNullException(nameof(authOptions.Value.AuthServerLocation)); }

            if (thisOptions?.Value == null) { throw new ArgumentException(nameof(thisOptions)); }
            if (String.IsNullOrWhiteSpace(thisOptions.Value.ClientId)){ throw new ArgumentNullException(nameof(thisOptions.Value.ClientId)); }
            if (String.IsNullOrWhiteSpace(thisOptions.Value.ClientSecret)){ throw new ArgumentNullException(nameof(thisOptions.Value.ClientSecret)); }
            
            if (logger == null){ throw new ArgumentNullException(nameof(logger)); }

            _authOptions = authOptions.Value;
            _clientOptions = thisOptions.Value;
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
        protected async Task SetBearerTokenAsync(HttpContext context)
        {
            string bearerToken;
            context.Request.Cookies.TryGetValue(AUTH_COOKIE_NAME, out bearerToken);

            if (String.IsNullOrWhiteSpace(bearerToken))
            {
                var disco = await DiscoveryClient.GetAsync(this._authOptions.AuthServerLocation);
                var tokenClient = new TokenClient(disco.TokenEndpoint, this._clientOptions.ClientId, this._clientOptions.ClientSecret);
                var tokenResponse = await tokenClient.RequestClientCredentialsAsync(this._authOptions.ApiName);

                if (tokenResponse.IsError)
                {
                    var message = "Error on receiving token for this Credential Auth";
                    Logger.LogError(message +": " + tokenResponse.Error);
                    throw new ArgumentException(message);
                }
                this.SetBearerToken(tokenResponse.AccessToken);
                context.Response.Cookies.Append(AUTH_COOKIE_NAME, tokenResponse.AccessToken, new CookieOptions()
                {
                    Expires = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn),
                    HttpOnly = true,
                    Secure = true
                });

                Logger.LogInformation($"Added new Access Token for Api {this._authOptions.ApiName}. Expires at {DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)}");
            }
            else
            {
                Logger.LogInformation($"Used cached Access Token for Api {this._authOptions.ApiName}");
                this.SetBearerToken(bearerToken);
            }
        }

        public async Task<HttpResponseMessage> PostAsJsonAsync<T>(string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await base.PostAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }

        public async Task<HttpResponseMessage> PutAsJsonAsync<T>(string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await base.PutAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }

        protected async Task<T> GetAsync<T>(string url, params object[] args)
        {
            var response = await this.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new ServerException(response);
        }

        protected async Task<HttpStatusCode> GetHttpStatusAsync(string url, params object[] args)
        {
            
            var response = await this.GetAsync(String.Format(url, args));
            return response.StatusCode;
        }

        protected async Task<byte[]> GetBytesAsync(string url, params object[] args)
        {
            
            var response = await this.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("GetBytesAsync<{0}>({1}) -> {2}", typeof(byte[]).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsByteArrayAsync();
            }
            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed GetBytesAsync<{0}>({1}) -> {2}{3}", typeof(byte[]).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response));

            throw new ServerException(response);
        }

        protected async Task PutAsJsonAsync<T>(T model, string url, params object[] args)
        {
            
            var response = await this.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled)
                //Log.Debug(String.Format("PutAsJsonAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled)
                //Log.Info(String.Format("Failed PutAsJsonAsync<{0}>({1}) -> {2}{3}", typeof(T).Name,
                //String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        protected async Task PutAsync(string url, params object[] args)
        {
            
            var response = await base.PutAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PutAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        protected async Task DeleteAsync(string url, params object[] args)
        {
            
            var response = await base.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("DeleteAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed DeleteAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        protected async Task<T> DeleteAsync<T>(string url, params object[] args)
        {
            
            var response = await base.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("DeleteAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<T>();
            }
            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed DeleteAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
            throw new ServerException(response);
        }

        protected async Task<T> PostAsync<T>(HttpContent content, string url, params object[] args)
        {
            
            var response = await base.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<T>();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PostAsync<{0}>({1}) -> {2}{3}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        protected async Task PostAsync(HttpContent content, string url, params object[] args)
        {
            
            var response = await base.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled)
                //Log.Debug(String.Format("PostAsync(HttpContent, {0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
            }
            else
            {
                //if (Log.IsInfoEnabled)
                //Log.Info(String.Format("Failed PostAsync(HttpContent, {0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        protected async Task PostAsync(string url, params object[] args)
        {
            
            var response = await base.PostAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                //response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));

                throw new ServerException(response);
            }
        }

        protected async Task PostAsJsonAsync<T>(T model, string url, params object[] args)
        {
            
            var response = await this.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync<{0}>({1}) -> {2}{3}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));

                throw new ServerException(response);
            }
        }

        protected async Task<TResult> PutAsJsonReturnAsync<T, TResult>(T model, string url, params object[] args)
        {
            
            var response = await this.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PutAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));

                return await response.Content.ReadAsAsync<TResult>();
            }

            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync<{0}, {1}>({2}) -> {3}{4}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
            throw new ServerException(response);
        }

        protected async Task<TResult> PostAsJsonReturnAsync<T, TResult>(T model, string url, params object[] args)
        {
            
            var response = await this.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<TResult>();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PostAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}{4}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        #endregion

        #region PROPERTIES
        public ILogger Logger { get; private set; }
        #endregion

    }
}
