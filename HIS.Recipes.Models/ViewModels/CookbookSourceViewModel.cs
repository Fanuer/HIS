using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class CookbookSourceViewModel : UpdateCookbookSourceViewModel
    {
        public CookbookSourceViewModel()
        {
            Recipes = new List<RecipeShortInfoViewModel>();
        }

        public IEnumerable<RecipeShortInfoViewModel> Recipes { get; set; }
    }
}
