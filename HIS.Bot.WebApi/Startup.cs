using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Bot.WebApi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Internal;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace HIS.Bot.WebApi
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
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }


        #endregion

        #region METHODS
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddLogging();
            #region Options
            services.AddOptions();
            services.Configure<HISClientInformation>(Configuration.GetSection("HISClientInfo"));
            //services.Configure<MsBotCrediential>(Configuration.GetSection("BotCredentials"));
            //services.Configure<AuthServerInfoOptions>(Configuration.GetSection("AuthServerInfo"));
            #endregion

            services.AddMemoryCache();
            var jsonSerialierSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented

            };

            services.AddMvcCore()
                .AddJsonFormatters()
                .AddJsonOptions(options =>{
                
                options.SerializerSettings.NullValueHandling = jsonSerialierSettings.NullValueHandling;
                options.SerializerSettings.ContractResolver = jsonSerialierSettings.ContractResolver;
                options.SerializerSettings.Formatting = jsonSerialierSettings.Formatting;
            });
            JsonConvert.DefaultSettings = () => new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Newtonsoft.Json.Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore,
            };
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseMvc(/*routes => { routes.MapRoute("DefaultApi", "api/{controller}/{id?}");}*/);
        }

        #endregion

        #region PROPERTIES
        public IConfigurationRoot Configuration { get; set; }

        #endregion
    }
}
