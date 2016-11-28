using HIS.Data.Base.Interfaces.Models;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces.SingleId
{
    public interface IRepositoryAddAndDelete<T, in TIdProperty> where T : class, IEntity<TIdProperty>
    {
        Task<T> AddAsync(T model);
        Task<bool> RemoveAsync(TIdProperty id);
        Task<bool> RemoveAsync(T model);
        Task<bool> ExistsAsync(TIdProperty id);
    }
}