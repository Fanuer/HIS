using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class StepViewModel: StepUpdateViewModel
    {
        [Range(0, int.MaxValue)]
        [Required]
        public int RecipeId { get; set; }
    }
}
