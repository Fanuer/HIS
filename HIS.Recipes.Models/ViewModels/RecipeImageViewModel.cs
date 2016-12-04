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
        public int Id { get; set; }
        [Required]
        public string Url { get; set; }

        public string Filename { get; set; }
    }
}
