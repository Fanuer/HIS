using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Configs.Converter
{
    internal class CookbookSourceConverter:ITypeConverter<RecipeCookbookSource, CookbookSourceViewModel>
    {
        public CookbookSourceViewModel Convert(RecipeCookbookSource source, CookbookSourceViewModel destination,ResolutionContext context)
        {
            var mapper = context.Mapper;
            var result = new CookbookSourceViewModel()
            {
                Id = source.Id,
                Name = source.Name,
                ISBN = source.ISBN,
                PublishingCompany = source.PublishingCompany
            };

            var infos = new List<RecipeSourceShortInfoViewModel>();

            foreach (var sourceRecipeSourceRecipe in source.RecipeSourceRecipes)
            {
                infos.Add(mapper.Map<RecipeSourceShortInfoViewModel>(sourceRecipeSourceRecipe));
            }
            result.Recipes = infos;

            return result;
        }
    }
}
