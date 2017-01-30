using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class RecipeCreationViewModel: RecipeChangeViewModel
    {
        
        [Required]
        public string Creator { get; set; }
        
    }
}
