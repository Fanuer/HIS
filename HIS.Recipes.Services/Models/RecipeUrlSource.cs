using System.ComponentModel.DataAnnotations;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeUrlSource:RecipeBaseSource
    {
        [Required]
        public string Url { get; set; }
    }
}
