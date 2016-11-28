using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class WebSourceCreationViewModel
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string SourceUrl { get; set; }
    }
}
