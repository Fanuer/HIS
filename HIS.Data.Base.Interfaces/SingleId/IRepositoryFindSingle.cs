using System;
using System.Linq.Expressions;
using HIS.Data.Base.Interfaces.Models;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces.SingleId
{
    public interface IRepositoryFindSingle<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> FindAsync<TProperty>(TIdProperty id, Expression<Func<T, TProperty>> navigationPropertyPath = null);
        Task<T> FindAsync(TIdProperty id);
    }
}