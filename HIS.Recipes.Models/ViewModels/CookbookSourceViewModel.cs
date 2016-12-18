using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class CookbookSourceViewModel : CookbookSourceCreationViewModel, IViewModelEntity<int>
    {
        public CookbookSourceViewModel()
        {
            Recipes = new List<RecipeShortInfoViewModel>();
        }

        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public string Url { get; set; }

        public IEnumerable<RecipeShortInfoViewModel> Recipes { get; set; }
    }
}
