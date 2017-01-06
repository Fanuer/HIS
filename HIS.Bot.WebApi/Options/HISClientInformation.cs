using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Bot.WebApi.Options
{
    public class HISClientInformation
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string AuthServerBaseUrl { get; set; }
        public string GatewayBaseUrl { get; set; }
        public string GatewayApiName { get; set; }
    }
}
