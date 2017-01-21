using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Options;
using IdentityModel.Client;
using Microsoft.Extensions.Configuration;

namespace HIS.Gateway.Tests
{
    public class TestBase
    {
        protected async Task<TokenResponse> GetAccessTokenFromAuthServer(ClientInformation clientInformation)
        {
            if (clientInformation == null) { throw new ArgumentNullException(nameof(clientInformation)); }

            // discover endpoints from metadata
            var disco = await DiscoveryClient.GetAsync(clientInformation.AuthServerLocation);

            // request token
            var tokenClient = new TokenClient(disco.TokenEndpoint, clientInformation.Credentials.ClientId, clientInformation.Credentials.ClientSecret);
            var tokenResponse = await tokenClient.RequestClientCredentialsAsync(clientInformation.TargetApiName);
            return tokenResponse;

        }
        protected IConfiguration GetConfig()
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.keys.json", optional: true, reloadOnChange: true);
            return builder.Build();
        }

    }
}
