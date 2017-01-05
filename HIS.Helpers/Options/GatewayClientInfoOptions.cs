using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Provides Configurations of a Gateway Api Client
    /// </summary>
    public class GatewayClientInfoOptions:ClientInfoOptions
    {
        /// <summary>
        /// List of all Apis with its BaseUrls to redirect incoming calls to 
        /// </summary>
        public Dictionary<string, string> GatewayClients { get; set; }
    }
}
