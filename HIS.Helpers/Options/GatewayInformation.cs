using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Provides Configurations of a Gateway Api Client
    /// </summary>
    public class GatewayInformation: ClientInformationBase
    {
        public GatewayInformation()
        {
            //GatewayClients = new Dictionary<string, string>(); 
        }

        /// <summary>
        /// List of all Apis with its BaseUrls to redirect incoming calls to 
        /// </summary>
        public Dictionary<string, string> GatewayClients { get; set; }

        public ClientInformation GetClientInformation(string apiName)
        {
            if (String.IsNullOrWhiteSpace(apiName)) { throw new ArgumentNullException(nameof(apiName));}
            if (this.GatewayClients == null || !this.GatewayClients.ContainsKey(apiName)) { throw new ArgumentOutOfRangeException($"No gateway client with Name '{apiName}' defined");}

            return new ClientInformation()
            {
                Credentials = this.Credentials,
                TargetApiName = apiName,
                AuthServerLocation = this.AuthServerLocation,
                TargetBaseUrl = this.GatewayClients[apiName]
            };
        }
    }
}
