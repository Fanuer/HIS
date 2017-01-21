using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Information of a Oauth2 Client
    /// </summary>
    public abstract class ClientInformationBase
    {
        public string AuthServerLocation { get; set; }

        /// <summary>
        /// Credentials to log in at the auth service. Is used for client credential flow.
        /// </summary>
        public ClientCredentials Credentials { get; set; }
    }
}
