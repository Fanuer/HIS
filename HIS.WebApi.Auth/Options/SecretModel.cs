using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Options
{
    /// <summary>
    /// Secret of an Identity Entity
    /// </summary>
    public class SecretModel
    {
        /// <summary>
        /// Entity Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Entity Secret
        /// </summary>
        public string Secret { get; set; }

        public DateTime ExpireDate { get; set; }
        public string Description { get; set; }
    }
}
