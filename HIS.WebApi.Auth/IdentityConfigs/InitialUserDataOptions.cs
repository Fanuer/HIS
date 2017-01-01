using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.WebApi.Auth.IdentityConfigs
{
    public class InitialUserDataOptions
    {
        public InitialUserDataOptions()
        {
            UserData = new List<UserData>();
        }
        public List<UserData> UserData { get; set; }
    }

    public class UserData
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
