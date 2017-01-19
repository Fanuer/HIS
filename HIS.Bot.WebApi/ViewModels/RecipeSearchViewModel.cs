using System.Collections.Generic;

namespace HIS.Bot.WebApi.ViewModels
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
    }
}
