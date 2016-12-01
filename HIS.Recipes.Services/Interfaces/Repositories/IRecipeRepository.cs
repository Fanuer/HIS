using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IRecipeRepository : IRepositoryFindAll<Recipe>, IRepositoryAddAndDelete<Recipe, Guid>, IRepositoryFindSingle<Recipe, Guid>, IRepositoryUpdate<Recipe, Guid>, IManualSaveChanges, IDisposable
    {
    }
}
