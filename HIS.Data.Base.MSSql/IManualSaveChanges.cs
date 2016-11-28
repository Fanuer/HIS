using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace HIS.Data.Base.MSSql
{
    public interface IManualSaveChanges
    {
        int SaveChanges();
        Task<int> SaveChangesAsync();
    }
}
