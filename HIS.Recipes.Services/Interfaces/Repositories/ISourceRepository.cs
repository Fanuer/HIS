using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface ISourceRepository : IRepositoryFindAll<RecipeBaseSource>, IRepositoryAddAndDelete<RecipeBaseSource, Guid>, IRepositoryFindSingle<RecipeBaseSource, Guid>, IRepositoryUpdate<RecipeBaseSource, Guid>
    {
    }
}
