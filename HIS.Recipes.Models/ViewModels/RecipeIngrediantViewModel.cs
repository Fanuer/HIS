using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeIngrediantViewModel:NamedViewModel
    {
        [Range(0, int.MaxValue)]
        public double? Amount { get; set; }
        public CookingUnit Unit { get; set; }
    }
}
