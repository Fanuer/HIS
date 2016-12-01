using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class ShortRecipeViewModel:NamedViewModel
    {
        public string ImageUrl { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}
