using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Data.Base.Interfaces
{
    public interface IFuzzySearchStore
    {
        Task<int> GetCachedFuzzyResultAsync(string searchQuery);
        Task SaveFuzzyResultAsync(FuzzyEntry newEntry);
    }
}
