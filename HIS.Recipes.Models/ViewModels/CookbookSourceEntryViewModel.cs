using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class CookbookSourceEntryViewModel: CookbookSourceViewModel
    {
        [Range(0, Int32.MaxValue)]
        public int Page { get; set; }
    }
}
