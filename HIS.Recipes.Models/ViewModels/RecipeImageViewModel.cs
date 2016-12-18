using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeImageViewModel:IViewModelEntity<int>
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public string Url { get; set; }
        [Required]
        public string Filename { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int RecipeId { get; set; }
    }
}
