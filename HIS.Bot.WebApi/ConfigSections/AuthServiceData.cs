using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HIS.Bot.WebApi.ConfigSections
{
    /// <summary>
    /// Defines data to access the HIS Auth Service
    /// </summary>
    public class AuthServiceData : ConfigurationElement
    {
        /// <summary>
        /// Base url of the Authentification Server
        /// </summary>
        [ConfigurationProperty("authServerLocation", DefaultValue = "", IsRequired = true)]
        public String AuthServerLocation
        {
            get
            {
                return (String)this["authServerLocation"];
            }
            set
            {
                this["authServerLocation"] = value;
            }
        }

        /// <summary>
        /// Shall HTTPS be used to communicate with the Auth Server
        /// </summary>
        [ConfigurationProperty("useHttps", DefaultValue = true, IsRequired = true)]
        public bool UseHttps
        {
            get
            {
                return (bool)this["useHttps"];
            }
            set
            {
                this["useHttps"] = value;
            }
        }

        /// <summary>
        /// Name of the Api, this client wants access to
        /// </summary>
        [ConfigurationProperty("apiName", DefaultValue = "", IsRequired = true)]
        public String ApiName
        {
            get
            {
                return (String)this["apiName"];
            }
            set
            {
                this["apiName"] = value;
            }
        }
    }
}