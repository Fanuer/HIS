using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeIngrediantViewModel:NamedViewModel
    {
        public int Amount { get; set; }
        public CookingUnit Unit { get; set; }
    }
}
