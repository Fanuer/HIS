using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    public class SourceListEntryViewModel:IViewModelEntity<int>
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Name { get; set; }
        public SourceType Type { get; set; }
        public int CountRecipes { get; set; }
    }
}
