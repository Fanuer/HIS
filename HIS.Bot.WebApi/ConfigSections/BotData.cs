using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace HIS.Bot.WebApi.ConfigSections
{
    public class BotData : ConfigurationSection
    {
        /// <summary>
        /// Defines data to login the client ciel OAuth2-ClientCredential-Authentification
        /// </summary>
        [ConfigurationProperty("clientInfo")]
        public ClientInfo ClientInfo
        {
            get
            {
                return (ClientInfo)this["clientInfo"];
            }
            set
            { this["clientInfo"] = value; }
        }

        /// <summary>
        /// Defines data to access the HIS Auth Service
        /// </summary>
        [ConfigurationProperty("authServiceData")]
        public AuthServiceData AuthServiceData
        {
            get
            {
                return (AuthServiceData)this["authServiceData"];
            }
            set
            { this["authServiceData"] = value; }
        }
    }
}