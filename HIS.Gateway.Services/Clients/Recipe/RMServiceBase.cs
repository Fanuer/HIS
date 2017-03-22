using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Options;
using HIS.Helpers.Web.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.Gateway.Services.Clients.Recipe
{
    /// <summary>
    /// Base class for all services that communicates with the recipe management
    /// </summary>
    internal abstract class RmServiceBase : S2SClientBase
    {
        #region CONST
        internal const string RecipeApiName = "recipe-api";
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        internal RmServiceBase(IOptions<GatewayInformation> clientOptions, ILogger logger) 
            : base(clientOptions.Value.GetClientInformation(RecipeApiName), logger)
        {
        }
        #endregion
    }
}
