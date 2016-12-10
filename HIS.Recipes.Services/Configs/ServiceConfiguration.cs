using HIS.Recipes.Services.DB;
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
        public static void AddServices(IServiceCollection services, IConfigurationRoot config, string recipeDbName )
        {
            
            services.AddDbContext<RecipeDbContext>(options => options.UseSqlServer(config.GetConnectionString(recipeDbName)));

            services.AddOptions();

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
