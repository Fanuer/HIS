using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeUpdateViewModel: RecipeChangeViewModel, IViewModelEntity<int>
    {
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
