using System.Linq;
using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces
{
    public interface IRepositoryFindAll<T>
    {
        IQueryable<T> GetAll();
    }
}