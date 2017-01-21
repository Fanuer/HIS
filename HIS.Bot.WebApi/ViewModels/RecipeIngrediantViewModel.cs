using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.ViewModels
{
    public class RecipeIngrediantViewModel:NamedViewModel
    {
        [Range(0, int.MaxValue)]
        public double? Amount { get; set; }
        public CookingUnit Unit { get; set; }
    }
}
