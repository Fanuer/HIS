using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IRecipeDBRepository
    {
        IDbImageRepository Images { get; }
        IIngrediantRepository Ingrediants { get; }
        IRecipeRepository Recipes { get; }
        ISourceRepository<RecipeCookbookSource> CookBookSources { get; }

        ISourceRepository<RecipeUrlSource> UrlSources { get; }
        IStepRepository Steps { get; }
        ITagsRepository Tags { get; }
    }
}
