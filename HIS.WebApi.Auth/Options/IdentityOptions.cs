using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.Options
{
    public class IdentityOptions
    {
        public List<SecretModel> Scopes { get; set; }
        public List<SecretModel> Clients { get; set; }
    }
}
