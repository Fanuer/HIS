using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IStepRepository:IRepositoryFindAll<RecipeStep>, IRepositoryAddAndDelete<RecipeStep, int>, IRepositoryUpdate<RecipeStep, int>, IRepositoryFindSingle<RecipeStep, int>, IDisposable
    {
        Task UpdateAllAsync(int recipeId, ICollection<string> entries);
    }
}
