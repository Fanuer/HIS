using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    /// <summary>
    /// Holds data to add or update an ingrediant on a recipe
    /// </summary>
    public class AlterIngrediantViewModel
    {
        /// <summary>
        /// Id of a recipe
        /// </summary>
        [Required]
        [Range(0, Int32.MaxValue)]
        public int RecipeId { get; set; }
        /// <summary>
        /// Id of an ingrediant
        /// </summary>
        [Required]
        [Range(0, Int32.MaxValue)]
        public int IngrediantId { get; set; }
        /// <summary>
        /// Amount of an ingrediant within a recipe
        /// </summary>
        [Required]
        public int Amount { get; set; }
        /// <summary>
        /// Cooking unit of the ingrediant
        /// </summary>
        [Required]
        public CookingUnit Unit { get; set; }
    }
}
