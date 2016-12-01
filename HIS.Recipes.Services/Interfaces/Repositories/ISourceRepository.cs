using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface ISourceRepository<T> : IDisposable, IRepositoryFindAll<T>, IRepositoryAddAndDelete<T, Guid>, IRepositoryFindSingle<T, Guid>, IRepositoryUpdate<T, Guid> where T: RecipeBaseSource
    {
    }

    internal interface IWebSourceRepository : ISourceRepository<RecipeUrlSource>
    {
    }

    internal interface ICookbookSourceRepository : ISourceRepository<RecipeCookbookSource>
    {
    }

    internal interface IBaseSourceRepository : ISourceRepository<RecipeBaseSource>
    {
    }

    internal interface IRecipeSourceRepository :IDisposable
    {
        IWebSourceRepository WebSources { get; }
        IBaseSourceRepository BaseSources { get; }
        ICookbookSourceRepository CookbookSources { get; }
    }
}
