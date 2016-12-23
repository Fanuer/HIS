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
        public string AuthServerLocation { get; set; }

        public bool UseHTTPS { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ApiName { get; set; }
    }
}
