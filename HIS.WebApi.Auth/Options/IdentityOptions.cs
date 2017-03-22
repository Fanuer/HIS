using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Options
{
    public class IdentityOptions
    {
        public List<SecretModel> ApiResources { get; set; }
        public List<SecretModel> Clients { get; set; }
        /// <summary>
        /// Password of the Certificate that is used to sign the tokens
        /// </summary>
        public string CertificatePassword { get; set; }
    }
}
