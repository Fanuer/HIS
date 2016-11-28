using HIS.Data.Base.Interfaces.Models;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces
{
    public interface IRepositoryUpdate<in T, TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<bool> UpdateAsync(T model);
    }
}