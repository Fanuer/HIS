using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    /// <summary>
    /// Holds ingrediant data
    /// </summary>
    public class IngrediantStatisticViewModel:NamedViewModel
    {
        /// <summary>
        /// Number of recipes for this ingrediant
        /// </summary>
        public int NumberOfRecipes { get; set; }
    }
}
