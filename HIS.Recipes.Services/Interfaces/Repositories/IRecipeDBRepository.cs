using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    interface IRecipeDBRepository
    {
        IDbImageRepository Images { get; }
        IIngrediantRepository Ingrediants { get; }
        IRecipeRepository Recipes { get; }
        ISourceRepository Sources { get; }
        IStepRepository Steps { get; }
        ITagsRepository Tags { get; }
    }
}
