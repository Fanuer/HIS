using System;
using System.Configuration;

namespace HIS.Bot.WebApi.XML.ConfigSections
{
    public class ClientInfo : ConfigurationElement
    {
        /// <summary>
        /// Unique id of the this client as stored in auth service
        /// </summary>
        [ConfigurationProperty("clientId", IsRequired = true)]
        public String ClientId
        {
            get
            {
                return (String)this["clientId"];
            }
            set
            {
                this["clientId"] = value;
            }
        }


        /// <summary>
        /// Secret of the client to verify authentification
        /// </summary>
        [ConfigurationProperty("clientSecret", IsRequired = true)]
        public String ClientSecret
        {
            get
            {
                return (String)this["clientSecret"];
            }
            set
            {
                this["clientSecret"] = value;
            }
        }

        /// <summary>
        /// Base uri of the api Gateway
        /// </summary>
        [ConfigurationProperty("gatewayUri", IsRequired = true)]
        public String GatewayUri
        {
            get
            {
                return (String)this["gatewayUri"];
            }
            set
            {
                this["gatewayUri"] = value;
            }
        }

    }
}