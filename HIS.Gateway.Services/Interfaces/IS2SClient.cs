using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HIS.Gateway.Services.Interfaces
{
    public interface IS2SClient
    {
        Task SetBearerTokenAsync(HttpContext context);
    }
}
