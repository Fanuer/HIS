using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Data.Base.Interfaces
{
    public interface IFuzzySearchStore<TFuzzy, TIdProperty> where TFuzzy : IFuzzyEntry<TIdProperty>
    {
        Task<IEnumerable<TIdProperty>> GetCachedFuzzyResultAsync(string type, string searchQuery);
        Task SaveFuzzyEntryAsync(TFuzzy newEntry);
        Task RemoveFuzzyEntryAsync(TFuzzy newEntry);
    }
}
