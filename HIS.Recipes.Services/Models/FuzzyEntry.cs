using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Models
{
    public class FuzzyEntry
    {
        public string SearchQuery { get; set; }
        public string Type { get; set; }
        public int EntityId { get; set; }
    }
}
