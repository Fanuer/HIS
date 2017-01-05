using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using HIS.WebApi.Auth.Data;
using HIS.WebApi.Auth.Models;
using HIS.WebApi.Auth.Services;
using System.Reflection;
using AutoMapper;
using HIS.WebApi.Auth.IdentityConfigs;
using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Entities;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyModel;

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
                .AddJsonFile("appsettings.keys.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true);

            builder.AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        #endregion

        #region METHODS
        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container. 
        /// </summary>
        /// <param name="services">Collection to add services to</param>
        public void ConfigureServices(IServiceCollection services)
        {
            var dbConnectionString = Configuration.GetConnectionString("DefaultConnection");

            // Add framework services.
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(dbConnectionString));

            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddMvc();

            // Add application services.
            services.AddTransient<IEmailSender, AuthMessageSender>();
            services.AddTransient<ISmsSender, AuthMessageSender>();

            var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name; // aktuelle Assembly
            services.AddIdentityServer()
                .AddTemporarySigningCredential()
                //.AddInMemoryPersistedGrants()
                
                .AddConfigurationStore(builder => builder.UseSqlServer(dbConnectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddOperationalStore(builder => builder.UseSqlServer(dbConnectionString, options => options.MigrationsAssembly(migrationsAssembly)))
                .AddAspNetIdentity<ApplicationUser>();

            services.AddAutoMapper();
            services.AddOptions();
            services.Configure<HIS.WebApi.Auth.Options.IdentityOptions>(Configuration.GetSection("Identity"));
            services.Configure<InitialUserDataOptions>(Configuration.GetSection("InitialUserData"));

            services.AddSingleton<IdentityConfig>();
        }
        
        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline. 
        /// </summary>
        /// <param name="app">Grants access to the app to add configurations</param>
        /// <param name="env">Grants access to the app enviroment to add configurations</param>
        /// <param name="loggerFactory">Logging Factory</param>
        /// <param name="identityConfig">Configuration of initial auth entities</param>
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IdentityConfig identityConfig)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                //InitializeDatabase(app, identityConfig);
                //InitialiseUsers(app);

                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseIdentity();
            app.UseIdentityServer();
            // Add external authentication middleware below. To configure them please see http://go.microsoft.com/fwlink/?LinkID=532715

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }


        private void InitialiseUsers(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>();
                if (context.Database.EnsureCreated())
                {
                    context.Database.Migrate();
                }
                string[] roles = {"Owner", "Administrator", "Editor", "User"};
                foreach (var role in roles)
                {
                    var roleStore = new RoleStore<IdentityRole>(context);
                    if (!String.IsNullOrWhiteSpace(role) && !context.Roles.Any(x=>x.Name.Equals(role)))
                    {
                        var createTask = roleStore.CreateAsync(new IdentityRole(role));
                        createTask.Wait();
                    }
                }

                if (!context.Users.Any())
                {
                    var usermanager = serviceScope.ServiceProvider.GetService<UserManager<ApplicationUser>>();

                    var initialusers = Configuration.GetSection("InitialUserData").Get<InitialUserDataOptions>();
                    if (usermanager != null && initialusers.UserData.Count > 0)
                    {
                        foreach (var userData in initialusers.UserData)
                        {
                            var user = new ApplicationUser() { UserName = userData.Username, SecurityStamp = Guid.NewGuid().ToString("D") };
                            var createTask = usermanager.CreateAsync(user, userData.Password);
                            createTask.Wait();
                            if (!createTask.Result.Succeeded)
                            {
                                foreach (var identityError in createTask.Result.Errors)
                                {
                                    throw new ArgumentException(identityError.Description);
                                }
                            }
                            /* var roleStore = new RoleStore<IdentityRole>(context);
                            roles = roleStore.Roles.Select(x => x.Name).ToArray();
                            var addRoleTask = usermanager.AddToRolesAsync(user, roles);
                            addRoleTask.Wait();*/
                        }
                    }
                }
                context.SaveChanges();
            }
        }

        private void InitializeDatabase(IApplicationBuilder app, IdentityConfig identityConfig)
        {
            using (var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                var persistantGrantDb = serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database;
                if (persistantGrantDb.EnsureCreated())
                {
                    persistantGrantDb.Migrate();
                }
                
                var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
                if (context.Database.EnsureCreated())
                {
                    context.Database.Migrate();
                }

                if (!context.ApiResources.Any())
                {
                    context.ApiResources.AddRange(identityConfig.GetApiResources().Select(x => x.ToEntity()));
                    context.SaveChanges();
                }
                if (!context.Clients.Any())
                {
                    context.Clients.AddRange(identityConfig.GetClients().Select(x => x.ToEntity()));
                    context.SaveChanges();
                }
            }
        }

        #endregion

        #region PROPERTIES

        public IConfigurationRoot Configuration { get; }

        #endregion
    }
}
