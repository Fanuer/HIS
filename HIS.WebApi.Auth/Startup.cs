using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace HIS.WebApi.Auth
{
    public class Startup
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddJsonFile($"appsettings.keys.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        #endregion

        #region METHODS
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddDeveloperIdentityServer()
                .AddInMemoryScopes(InMemoryData.GetScopes())
                .AddInMemoryClients(InMemoryData.GetClients())
                .AddInMemoryUsers(InMemoryData.GetUsers());

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));

            if (env.IsDevelopment())
            {
                loggerFactory.AddDebug();
                app.UseDeveloperExceptionPage();
            }


            app.UseIdentityServer();
        }

        #endregion

        #region PROPERTIES
        public IConfigurationRoot Configuration { get; }

        #endregion

        

    }
}
