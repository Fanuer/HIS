using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Options;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using HIS.Helpers.Extensions;
using HIS.Helpers.Web.Clients;
using Microsoft.AspNetCore.WebUtilities;

namespace HIS.Gateway.Services.Clients
{
    /// <summary>
    /// A Client within the Gateway that calls the recipe-api
    /// </summary>
    internal partial class GatewayRecipeClient:S2SClientBase, IGatewayRecipeClient
    {

        #region CONST

        private const string apiName = "recipe-api";
        #endregion

        #region FIELDS
        #endregion

        #region CTOR        
        public GatewayRecipeClient(IOptions<GatewayInformation> clientOptions, ILoggerFactory factory) 
            : base(clientOptions.Value.GetClientInformation(apiName), factory.CreateLogger<GatewayRecipeClient>())
        {
        }

        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        #endregion

    }
}
