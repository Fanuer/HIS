using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.Services.Configs
{
    public static class ServiceConfiguration
    {
        /// <summary>
        /// Registers all services to the DI
        /// </summary>
        /// <param name="services">ServiceCollection to add the services to</param>
        /// <param name="config">AppSettings-Configuration</param>
        /// <param name="recipeDbName">Name of the Db ConnectionString within the AppSettings-Configuration</param>
        /// <param name="blobStoreSectionName">Name of the Subsection within the AppSettings which stores the data for the Azure Blob Storage</param>
        public static void AddServices(IServiceCollection services, IConfigurationRoot config, string recipeDbName, string blobStoreSectionName= "AzureBlobStorage")
        {
            services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(config.GetConnectionString(recipeDbName)));

            services.AddOptions();
            services.Configure<AzureBlobStorageOptions>(config.GetSection(blobStoreSectionName));

            // Repositories
            services.AddScoped<IDbImageRepository, RecipeDbRepository.DbImageRepository>();
            services.AddScoped<IIngrediantRepository, RecipeDbRepository.IngrediantRepository>();
            services.AddScoped<IRecipeRepository, RecipeDbRepository.RecipeRepository>();
            services.AddScoped<ICookbookSourceRepository, RecipeDbRepository.CookbookSourceRepository>();
            services.AddScoped<IWebSourceRepository, RecipeDbRepository.WebSourceRepository>();
            services.AddScoped<IBaseSourceRepository, RecipeDbRepository.BaseSourceRepository>();
            services.AddScoped<IRecipeSourceRepository, RecipeSourceRepository>();
            services.AddScoped<IStepRepository, RecipeDbRepository.StepRepository>();
            services.AddScoped<ITagsRepository, RecipeDbRepository.TagsRepository>();
            services.AddScoped<IRecipeDBRepository, RecipeDbRepository>();

            // Services
            services.AddTransient<IImageService, AzureImageService>();
            services.AddTransient<IRecipeImageService, RecipeImagesService>();
            services.AddTransient<IRecipeStepService, RecipeStepService>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<ISourceService, SourceService>();
            services.AddTransient<ITagService, TagService>();
            services.AddTransient<IIngrediantService, IngrediantService>();
        }
    }
}
