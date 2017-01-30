using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    public class FuzzyEntry:IFuzzyEntry
    {
        public string SearchQuery { get; set; }
        public IEntity<int> Entity { get; set; }
    }
}
