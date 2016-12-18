using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class ShortRecipeViewModel:NamedViewModel
    {
        public ShortRecipeViewModel()
        {
            Tags = new List<string>();
        }
        [Required]
        public string Creator { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public List<string> Tags { get; set; }
    }
}
