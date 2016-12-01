using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class FullRecipeViewModel:RecipeUpdateViewModel 
    {
        public int CookedCounter { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public IEnumerable<NamedViewModel> Images { get; set; }
        public IEnumerable<NamedViewModel> Tags { get; set; }
        public IEnumerable<StepViewModel> Steps { get; set; }
        public IEnumerable<RecipeIngrediantViewModel> Ingrediants { get; set; }
        public RecipeSourceShortInfoViewModel Type { get; set; }
    }
}
