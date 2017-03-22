using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using HIS.WebApi.Auth.Options;
using IdentityModel;
using IdentityServer4.Models;
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
                    // this API allows only following scopes
                    //Scopes = new List<Scope>() {new Scope("recipeUser", "A regular User"), new Scope("recipeAdmin", "An administrative User") }
                },
                new ApiResource("ha-api", "Api to interact with an openhab home automatisation")
                {
                    // include the following using claims in access token (in addition to subject id)
                    UserClaims = {JwtClaimTypes.Name, JwtClaimTypes.Email},
                    // secret for using introspection endpoint
                    ApiSecrets = new List<Secret>() {new Secret(_options.ApiResources.First(x=>x.Name.Equals("recipe-api")).Secret.Sha256()) },
                    // this API allows only following scopes
                    //Scopes = new List<Scope>() {new Scope("recipeUser", "A regular User"), new Scope("recipeAdmin", "An administrative User") }
                }, 
                // no secret needed: all clients, known to the Auth Service call access the gateway
                new ApiResource("gateway-resource", "Api Gateway Resource")
                {
                    ApiSecrets = new List<Secret>() {new Secret(_options.ApiResources.First(x=>x.Name.Equals("gateway-resource")).Secret.Sha256()) },
                    Scopes = new List<Scope>() {new Scope("recipe-management-scope", "Scope for the recipe Api Resource"), new Scope("ha-scope", "Scope for the Home Automatisation Api Resource") },
                    UserClaims = { "role", "his.admin", "his.user", "his.recipes", "his.recipes.write", "his.recipes.read" }
                }
            };
        }

        public IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>()
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource("recipe-management-scope", new List<string>() {"role", "his.admin", "his.user", "his.recipes", "his.recipes.write", "his.recipes.read"})
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
                    AllowedScopes = new List<string>() { "recipe-api", "ha-api" } // scopes that client has access to
                },
                new Client()
                {
                    ClientId = "bot-client",
                    ClientName = "Bot Application Client",
                    RequireConsent = false,
                    AllowedGrantTypes = GrantTypes.ImplicitAndClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("bot-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" }
                },
                new Client()
                {
                    ClientId = "app-client",
                    ClientName = "app Client",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPasswordAndClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("app-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" }
                },
                new Client()
                {
                    ClientId = "spa-client",
                    ClientName = "Angular2 App",
                    RequireConsent = false,
                    AccessTokenType = AccessTokenType.Reference,
                    AllowAccessTokensViaBrowser = true, 
                    AllowedGrantTypes = GrantTypes.Implicit,
                    
                    RedirectUris =
                    {
                        "https://his-spa.azurewebsites.net/unauthorized",
                        "http://localhost:8080/unauthorized"
                    },
                    PostLogoutRedirectUris  =
                    {
                        "https://his-spa.azurewebsites.net/unauthorized",
                        "http://localhost:8080/unauthorized"
                    },
                    AllowedCorsOrigins  =
                    {
                        "https://his-spa.azurewebsites.net/",
                        "http://localhost:8080"
                    },

                    AllowedScopes = new List<string>()
                    {
                        IdentityServer4.IdentityServerConstants.StandardScopes.OpenId, 
                        IdentityServer4.IdentityServerConstants.StandardScopes.Profile, 
                        IdentityServer4.IdentityServerConstants.StandardScopes.Email, 
                        "gateway-resource"
                    }
                },


                #region TestClients
                new Client()
                {
                    ClientId = "recipe-api-test-client",
                    ClientName = "Recipe Api Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("recipe-api-test-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "recipe-api" } // scopes that client has access to
                },
                new Client()
                {
                    ClientId = "ha-api-test-client",
                    ClientName = "Home Automatisation Api Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("ha-api-test-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "ha-api" } // scopes that client has access to
                },
                new Client()
                {
                    ClientId = "api-gateway-test-client",
                    ClientName = "Api Gateway Test Client",
                    AllowedGrantTypes = GrantTypes.ClientCredentials,
                    ClientSecrets = new List<Secret>() {new Secret(_options.Clients.First(x=>x.Name.Equals("api-gateway-test-client")).Secret.Sha256()) },
                    AllowedScopes = new List<string>() { "gateway-resource" } // scopes that client has access to
                },
                #endregion
            };
        }

        #endregion

        #region PROPERTIES
        
        #endregion
    }
}
