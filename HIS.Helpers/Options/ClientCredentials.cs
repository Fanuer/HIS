using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Helpers.Options
{
    /// <summary>
    /// Provides Configuration of an Client that uses ClientCredential-Authorisation
    /// </summary>
    public class ClientCredentials
    {
        /// <summary>
        /// Name of the this client
        /// </summary>
        public string ClientId { get; set; }
        /// <summary>
        /// Secret of the client to use for ClientCredential-Authorisation
        /// </summary>
        public string ClientSecret { get; set; }
    }
}
