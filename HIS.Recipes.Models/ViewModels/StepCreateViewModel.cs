using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class StepCreateViewModel
    {
        [Range(0, int.MaxValue)]
        public int Order { get; set; }

        [Required]
        public string Description { get; set; }
    }
}
