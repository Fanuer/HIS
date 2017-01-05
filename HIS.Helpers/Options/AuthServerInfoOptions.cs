using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Information used by an Api Resource to authentificate incoming requests
    /// </summary>
    public class AuthServerInfoOptions
    {
        /// <summary>
        /// Base url of the Authentification Server
        /// </summary>
        public string AuthServerLocation { get; set; }

        /// <summary>
        /// Shall HTTPS be used to communicate with the Auth Server
        /// </summary>
        public bool UseHttps { get; set; }

        /// <summary>
        /// Name of the Api of this service
        /// </summary>
        public string ApiName { get; set; }
    }
}
