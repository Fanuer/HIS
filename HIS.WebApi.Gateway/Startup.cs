using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Helpers.Options;
using HIS.WebApi.Gateway.Clients;
using HIS.WebApi.Gateway.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HIS.WebApi.Gateway
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
                .AddJsonFile("appsettings.keys.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);


            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        #endregion

        #region METHODS
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<GatewayClientInfoOptions>(Configuration.GetSection("ClientInfo"));
            services.Configure<AuthServerInfoOptions>(Configuration.GetSection("AuthServerInfo"));

            services.AddScoped<IRecipeBotClient, RecipeBotClient>();
            services.AddLogging();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016, 12, 18), 1, 0);
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AuthServerInfoOptions> authServerInfo)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authServerInfo.Value.AuthServerLocation,
                RequireHttpsMetadata = authServerInfo.Value.UseHTTPS,
                ApiName = authServerInfo.Value.ApiName
            });

            app.UseMvc();
        }

        #endregion

        #region PROPERTIES
        public IConfigurationRoot Configuration { get; set; }

        #endregion

    }
}
