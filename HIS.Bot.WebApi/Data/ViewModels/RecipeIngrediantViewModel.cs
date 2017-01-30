using System.ComponentModel.DataAnnotations;
using HIS.Bot.WebApi.Data.ViewModels.Enum;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class RecipeIngrediantViewModel:NamedViewModel
    {
        [Range(0, int.MaxValue)]
        public double? Amount { get; set; }
        public CookingUnit Unit { get; set; }
    }
}
