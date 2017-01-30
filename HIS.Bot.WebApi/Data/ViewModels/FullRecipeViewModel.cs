using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Bot.WebApi.Data.ViewModels.Interfaces;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class FullRecipeViewModel:RecipeCreationViewModel, IViewModelEntity<int>
    {
        public FullRecipeViewModel()
        {
            Images = new List<NamedViewModel>();
            Tags = new List<NamedViewModel>();
            Steps = new List<StepViewModel>();
            Ingrediants = new List<RecipeIngrediantViewModel>();
            Source = new RecipeSourceShortInfoViewModel();
        }

        [Range(0, int.MaxValue)]
        public int CookedCounter { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public IEnumerable<NamedViewModel> Images { get; set; }
        public IEnumerable<NamedViewModel> Tags { get; set; }
        public IEnumerable<StepViewModel> Steps { get; set; }
        public IEnumerable<RecipeIngrediantViewModel> Ingrediants { get; set; }
        public RecipeSourceShortInfoViewModel Source { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        public string Url { get; set; }
    }
}
