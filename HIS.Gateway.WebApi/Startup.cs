using System;
using System.IO;
using HIS.Gateway.Services.Configs;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;

namespace HIS.Gateway.WebApi
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
            services.Configure<GatewayInformation>(Configuration.GetSection("ClientInfo"));
            services.Configure<AuthServerInfoOptions>(Configuration.GetSection("AuthServerInfo"));

            ServiceConfiguration.AddServices(services, Configuration);

            services.AddLogging();

            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016, 12, 18), 1, 0);
            });

            #region Swagger

            // Inject an implementation of ISwaggerProvider with defaulted settings applied.
            services.AddSwaggerGen();

            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "HIS API Gateway",
                    Description = "Api Gateways to interact with Apis of the Home Information System",
                });

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "HIS.Gateway.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
                options.DescribeAllEnumsAsStrings();
            });


            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AuthServerInfoOptions> authServerInfo)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug(LogLevel.Trace);
            loggerFactory.AddAzureWebAppDiagnostics();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authServerInfo.Value.AuthServerLocation,
                RequireHttpsMetadata = authServerInfo.Value.UseHttps,
                ApiName = authServerInfo.Value.ApiName
            });

            app.UseMvcWithDefaultRoute();
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();

        }

        #endregion

        #region PROPERTIES
        public IConfigurationRoot Configuration { get; set; }

        #endregion

    }
}
