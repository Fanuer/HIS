using System.ComponentModel.DataAnnotations;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeCookbookSource:RecipeBaseSource
    {
        [Required]
        public string PublishingCompany { get; set; }
        
        public string ISBN { get; set; }
    }
}
