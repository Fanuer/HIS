using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Gateway.Services.Clients;
using HIS.Gateway.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Gateway.Services.Configs
{
    public class ServiceConfiguration
    {
        /// <summary>
        /// Registers all services to the DI
        /// </summary>
        /// <param name="services">ServiceCollection to add the services to</param>
        /// <param name="config">AppSettings-Configuration</param>
        public static void AddServices(IServiceCollection services, IConfigurationRoot config)
        {
            services.AddScoped<IGatewayRecipeClient, GatewayRecipeClient>();
        }
    }
}
