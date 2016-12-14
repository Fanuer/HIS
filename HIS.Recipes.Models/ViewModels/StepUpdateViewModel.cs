using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class StepUpdateViewModel : StepCreateViewModel
    {
        [Required]
        public int Id { get; set; }
    }
}
