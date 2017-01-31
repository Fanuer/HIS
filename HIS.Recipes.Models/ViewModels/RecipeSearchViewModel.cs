using System;
using System.Collections.Generic;
using System.Linq;

namespace HIS.Recipes.Models.ViewModels
{
    public class RecipeSearchViewModel
    {
        public RecipeSearchViewModel()
        {
            Tags = new List<string>();
            Ingrediants = new List<string>();
        }
        /// <summary>
        /// The name or a factment of a recipes name, the search is case insensitive
        /// </summary>
        public string Name { get; set; }
        public IEnumerable<string> Tags { get; set; }
        public IEnumerable<string> Ingrediants { get; set; }

        public bool IsFilled()
        {
            return !String.IsNullOrWhiteSpace(this.Name) ||
                   (this.Tags != null && this.Tags.Any()) ||
                   (this.Ingrediants != null && this.Ingrediants.Any());
        }
    }
}
