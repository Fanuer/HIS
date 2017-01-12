using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Models
{
    public class ListViewModel<T> where T:class
    {
        public int TotalCount { get; set; }
        public int Page { get; set; }
        public int EntriesPerPage { get; set; }
        public IEnumerable<T> Entries { get; set; }
    }
}
