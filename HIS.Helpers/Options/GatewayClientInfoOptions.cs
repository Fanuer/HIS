using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    public class GatewayClientInfoOptions:ClientInfoOptions
    {
        public Dictionary<string, string> GatewayClients { get; set; }
    }
}
