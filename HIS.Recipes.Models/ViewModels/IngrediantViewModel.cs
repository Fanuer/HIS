using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    /// <summary>
    /// Holds data of an recipe ingrediant
    /// </summary>
    public class IngrediantViewModel:NamedViewModel
    {
        /// <summary>
        /// Amount of an ingrediant within a recipe
        /// </summary>
        [Required]
        [Range(0, int.MaxValue)]
        public double? Amount { get; set; }
        /// <summary>
        /// Cooking unit of the ingrediant
        /// </summary>
        [Required]
        public CookingUnit Unit { get; set; }
    }
}
