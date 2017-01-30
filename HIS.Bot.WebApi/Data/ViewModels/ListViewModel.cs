using System.Collections.Generic;
using System.Linq;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class ListViewModel<T> where T:class
    {
        public ListViewModel(): this(null)
        {
            
        }

        public ListViewModel(IEnumerable<T> entries, int totalCount = -1, int page = 1, int entriesPerPage = 10)
        {
            Entries = entries ?? new List<T>();
            TotalCount = totalCount == -1 ? Entries.Count() : totalCount;
            Page = page;
            EntriesPerPage = entriesPerPage;
        }

        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int EntriesPerPage { get; set; }
        public IEnumerable<T> Entries { get; set; }
    }
}
