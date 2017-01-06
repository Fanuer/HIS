using System;
using System.IO;
using AutoMapper;
using HIS.Helpers.Options;
using HIS.Helpers.Web.Filters;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.Swagger.Model;

namespace HIS.Recipes.WebApi
{
    public class Startup
    {

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.keys.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container.
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            // Add framework services.
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(CheckForInvalidModelFilter));
                options.Filters.Add(typeof(DataObjectNotFoundExceptionFilter));
                options.Filters.Add(typeof(IdsNotIdenticalExceptionFilter));
            });

            services.AddOptions();
            services.AddLogging();
            services.AddApiVersioning(o =>
            {
                o.AssumeDefaultVersionWhenUnspecified = true;
                o.DefaultApiVersion = new ApiVersion(new DateTime(2016,12,18), 1, 0);
            });

            services.AddAutoMapper();
            Services.Configs.ServiceConfiguration.AddServices(services, Configuration, "DefaultConnection");
            services.Configure<AuthServerInfoOptions>(Configuration.GetSection("AuthServerInfo"));

            // Inject an implementation of ISwaggerProvider with defaulted settings applied.
            services.AddSwaggerGen();

            services.ConfigureSwaggerGen(options =>
            {
                options.SingleApiVersion(new Info
                {
                    Version = "v1",
                    Title = "Recipe API",
                    Description = "Api to interact with the Recipe management",
                });

                //Determine base path for the application.
                var basePath = PlatformServices.Default.Application.ApplicationBasePath;

                //Set the comments path for the swagger json and ui.
                var xmlPath = Path.Combine(basePath, "HIS.Recipes.WebApi.xml");
                options.IncludeXmlComments(xmlPath);
            });
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app">App Builder</param>
        /// <param name="env">Enviroment Context</param>
        /// <param name="loggerFactory">Logger Factory</param>
        /// <param name="authServerInfo">Information of used auth server</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<AuthServerInfoOptions> authServerInfo)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIdentityServerAuthentication(new IdentityServerAuthenticationOptions
            {
                Authority = authServerInfo.Value.AuthServerLocation,
                ScopeName = authServerInfo.Value.ApiName,
                RequireHttpsMetadata = authServerInfo.Value.UseHttps && !env.IsDevelopment() // https only in production
            });

            app.UseMvcWithDefaultRoute();
            // Enable middleware to serve generated Swagger as a JSON endpoint
            app.UseSwagger();
            // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
            app.UseSwaggerUi();
        }
    }
}
