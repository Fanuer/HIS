using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public abstract class RecipeChangeViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int NumberOfServings { get; set; }
        [Range(0, int.MaxValue)]
        public int? Calories { get; set; }
    }
}
