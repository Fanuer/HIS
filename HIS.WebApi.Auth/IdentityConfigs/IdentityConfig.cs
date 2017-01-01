using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using IdentityServer4.Models;
using Microsoft.Extensions.Options;
using HIS.WebApi.Auth.Options;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;

namespace HIS.WebApi.Auth.IdentityConfigs
{
    public class IdentityConfig
    {
        #region CONST
        #endregion

        #region FIELDS

        private readonly IdentityOptions _options;
        #endregion

        #region CTOR
        public IdentityConfig(IOptions<IdentityOptions> options)
        {
            if (options.Value== null){ throw new ArgumentNullException(nameof(options)); }
            _options = options?.Value;
        }
        #endregion

        #region METHODS
        public IEnumerable<ApiResource> GetApiResources()
        {
            // Apöi Resources represent data endpoints
            return new List<ApiResource>()
            {
                new ApiResource("recipe-api", "Api to interact with a recipe management")
                {
                    // include the following using claims in access token (in addition to subject id)
                    UserClaims = {JwtClaimTypes.Name, JwtClaimTypes.Email},
                    // secret for using introspection endpoint
                    ApiSecrets = new List<Secret>() {new Secret(_options.ApiResources.First(x=>x.Name.Equals("recipe-api")).Secret.Sha256()) },
                    // this API defines two scopes
                    Scopes = new List<Scope>() {new Scope("recipeUser", "A regular User"), new Scope("recipeAdmin", "An administrative User") }
                }, 
                // no secret needed: all clients, known to the Auth Service call access the gateway
                new ApiResource("gateway-resource", "Api Gateway Resource")
            };
        }

        public IEnumerable<Client> GetClients()
        {
            return new List<Client>()
            {
                new Client()
                {
                    ClientId ="recipe-client",
                    ClientName = "Recipe Service Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("recipe-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" } // scopes that client has access to
                },
                new Client()
                {
                    ClientId = "gateway-client",
                    ClientName = "Api Gateway Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("gateway-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "recipe-api" } // scopes that client has access to
                },
                new Client()
                {
                    ClientId = "bot-client",
                    ClientName = "Bot Application Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("bot-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" }
                },
                new Client()
                {
                    ClientId = "app-client",
                    ClientName = "app Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("app-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" }
                },
                new Client()
                {
                    ClientId = "spa-client",
                    ClientName = "Bot Application",
                    AllowedGrantTypes = GrantTypes.Implicit,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("spa-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" }
                }
            };
        }

        #endregion

        #region PROPERTIES
        
        #endregion
    }
}
