using System;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IDbImageRepository : IRepositoryFindAll<RecipeImage>, IRepositoryAddAndDelete<RecipeImage, int>, IRepositoryFindSingle<RecipeImage, int>, IRepositoryUpdate<RecipeImage, int>, IDisposable
    {
    }
}
