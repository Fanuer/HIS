using System;
using HIS.Data.Base.Interfaces;
using HIS.Data.Base.Interfaces.SingleId;
using HIS.Data.Base.MSSql;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Interfaces.Repositories
{
    internal interface IIngrediantRepository : IRepositoryFindAll<Ingrediant>, IRepositoryAddAndDelete<Ingrediant, Guid>, IRepositoryFindSingle<Ingrediant, Guid>, IRepositoryUpdate<Ingrediant, Guid>, IDisposable, IManualSaveChanges
    {
    }
}
