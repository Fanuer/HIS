using System.ComponentModel.DataAnnotations;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public abstract class RecipeChangeViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [Range(0, int.MaxValue)]
        public int NumberOfServings { get; set; }
        [Range(0, int.MaxValue)]
        public int? Calories { get; set; }
    }
}
