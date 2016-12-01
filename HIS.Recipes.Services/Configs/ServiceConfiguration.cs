using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.Extensions.DependencyInjection;

namespace HIS.Recipes.Services.Configs
{
    public static class ServiceConfiguration
    {
        public static void AddServices(IServiceCollection services)
        {
            services.AddDbContext<RecipeDBContext>();

            // Services
            services.AddTransient<IImageService, AzureImageService>();
            services.AddTransient<IRecipeImageService, RecipeImagesService>();
            services.AddTransient<IRecipeStepService, RecipeStepService>();
            services.AddTransient<IRecipeService, RecipeService>();
            services.AddTransient<ISourceService, SourceService>();


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
        }
    }
}
