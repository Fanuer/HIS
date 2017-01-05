using System;
using System.Collections.Generic;
using HIS.Helpers.Options;
using Microsoft.Extensions.Configuration;

namespace HIS.Gateway.Tests
{
    public class GatewayInformation
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public GatewayInformation(IConfiguration config)
        {
            if (config == null) { throw new ArgumentNullException(nameof(config)); }
            this.ClientInfo = GetClientInfos(config, "ClientInfo");
            this.ExternalClientInfo = GetClientInfos(config, "ExternalClientInfo");
            this.AuthServerUrl = GetConfigKey(config, "AuthServerUrl");
            this.GatewayApiBaseUrl = GetConfigKey(config, "GatewayApiBaseUrl");
            this.GatewayApiName = GetConfigKey(config, "GatewayApiName");
            this.Apis = GetApis(config);
        }
        #endregion

        #region METHODS
        
        private ClientInfoOptions GetClientInfos(IConfiguration config, string sectionKey)
        {
            if (String.IsNullOrWhiteSpace(sectionKey)) { throw new ArgumentNullException(nameof(sectionKey));}

            var result = new ClientInfoOptions();
            var configClientInfo = config.GetSection(sectionKey);
            result.ClientId = configClientInfo["ClientId"];
            result.ClientSecret = configClientInfo["ClientSecret"];

            if (String.IsNullOrWhiteSpace(result.ClientId)) { throw new ArgumentNullException(nameof(result.ClientId), "Appsettings.Keys.Json-Configfile must define the key ClientInfo:ClientId"); }
            if (String.IsNullOrWhiteSpace(result.ClientSecret)) { throw new ArgumentNullException(nameof(result.ClientSecret), "Appsettings.Keys.Json-Configfile must define the key ClientInfo:ClientSecret"); }
            return result;
        }

        private Dictionary<string, string> GetApis(IConfiguration config)
        {
            var result = new Dictionary<string, string>();
            var apis = config.GetSection("Apis").GetChildren();
            foreach (var section in apis)
            {
                var apiName = section["Name"];
                if (!String.IsNullOrWhiteSpace(apiName))
                {
                    var baseUrl = section["BaseUrl"];
                    if (!result.ContainsKey(apiName))
                    {
                        result.Add(apiName, baseUrl);
                    }
                }
            }
            return result;
        }

        private string GetConfigKey(IConfiguration config, string key)
        {
            if (config == null) { throw new ArgumentNullException(nameof(config)); }
            if (String.IsNullOrWhiteSpace(key)) { throw new ArgumentNullException(nameof(key)); }
            var result = config[key];
            if (String.IsNullOrWhiteSpace(result)) { throw new ArgumentNullException(key, $"Appsettings.Keys.Json-Configfile must define the key '{key}'"); }
            return result;
        }

        #endregion

        #region PROPERTIES
        public ClientInfoOptions ExternalClientInfo { get; }
        public ClientInfoOptions ClientInfo { get; }
        public string GatewayApiBaseUrl { get; }
        public string AuthServerUrl { get; }
        public string GatewayApiName { get; }
        public Dictionary<string, string> Apis { get; }

        #endregion
    }
}
