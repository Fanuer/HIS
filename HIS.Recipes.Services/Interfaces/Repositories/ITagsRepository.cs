using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface ITagsRepository : IRepositoryFindAll<RecipeTag>, IRepositoryAddAndDelete<RecipeTag, Guid>, IRepositoryFindSingle<RecipeTag, Guid>, IRepositoryUpdate<RecipeTag, Guid>, IManualSaveChanges, IDisposable
    {
    }
}
