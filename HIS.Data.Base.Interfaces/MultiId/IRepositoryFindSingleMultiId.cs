using HIS.Data.Base.Interfaces.Models;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces.MultiId
{
    public interface IRepositoryFindSingleMultiId<T, in TIdProperty> where T : IEntity<TIdProperty>
    {
        Task<T> FindAsync(TIdProperty[] id);
    }
}