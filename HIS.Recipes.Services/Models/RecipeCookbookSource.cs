using System.ComponentModel.DataAnnotations;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeCookbookSource:RecipeBaseSource
    {
        [Required]
        public string PublishingCompany { get; set; }
        [Required]
        [RegularExpression("^(ISBN[-]*(1[03])*[ ]*(: ){0,1})*(([0-9Xx][- ]*){13}|([0-9Xx][- ]*){10})$")]
        public string ISBN { get; set; }
    }
}
