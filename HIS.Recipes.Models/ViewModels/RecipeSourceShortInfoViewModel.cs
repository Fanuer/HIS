using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeSourceShortInfoViewModel:NamedViewModel
    {
        public int? Page { get; set; }
        public SourceType Type { get; set; }
    }
}
