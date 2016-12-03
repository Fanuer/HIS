using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    /// <summary>
    /// Almost identical to <see cref="RecipeShortInfoViewModel"/>. Dublicate to map from data from recipe to source and source to recipe.
    /// Within this class name and id are used for recipe data
    /// </summary>
    public class RecipeShortInfoViewModel : NamedViewModel
    {
        public int? Page { get; set; }
        public SourceType Type { get; set; }
    }
}
