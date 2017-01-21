using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Information of a Oauth2 Client
    /// </summary>
    public class ClientInformation:ClientInformationBase
    {
        /// <summary>
        /// Name of the Api, this client wants access to
        /// </summary>
        public string TargetApiName { get; set; }
        /// <summary>
        /// Base url of the resource service, this client wants access to
        /// </summary>
        public string TargetBaseUrl { get; set; }
    }
}
