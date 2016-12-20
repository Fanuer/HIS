using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Models.ViewModels
{
    public class SourceListEntryViewModel
    {
        [Required]
        [Range(0, int.MaxValue)]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public SourceType Type { get; set; }
        [Range(0, int.MaxValue)]
        public int CountRecipes { get; set; }
    }
}
