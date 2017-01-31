using System;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IRecipeRepository : IRepositoryFindAll<Recipe>, IRepositoryAddAndDelete<Recipe, int>, IRepositoryFindSingle<Recipe, int>, IRepositoryUpdate<Recipe, int>, IManualSaveChanges, IDisposable, IFuzzySearchStore<FuzzyEntry, int>
    {
        
    }
}
