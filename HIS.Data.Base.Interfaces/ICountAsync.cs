using System.Threading.Tasks;

namespace HIS.Data.Base.Interfaces
{
  public interface ICountAsync
  {
      Task<int> CountAsync();
  }
}
