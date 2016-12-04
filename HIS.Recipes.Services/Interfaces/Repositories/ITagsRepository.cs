using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface ITagsRepository : IRepositoryFindAll<RecipeTag>, IRepositoryAddAndDelete<RecipeTag, int>, IRepositoryFindSingle<RecipeTag, int>, IRepositoryUpdate<RecipeTag, int>, IManualSaveChanges, IDisposable
    {
    }
}
