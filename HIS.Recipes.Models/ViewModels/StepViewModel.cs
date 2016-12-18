using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class StepViewModel: StepUpdateViewModel
    {
        [Range(0, int.MaxValue)]
        [Required]
        public int RecipeId { get; set; }
    }
}
