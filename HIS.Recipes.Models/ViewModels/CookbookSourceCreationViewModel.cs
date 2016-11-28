using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Models.ViewModels
{
    public class CookbookSourceCreationViewModel
    {
        [Required]
        public string PublishingCompany { get; set; }
        [Required]
        [RegularExpression("^(ISBN[-]*(1[03])*[ ]*(: ){0,1})*(([0-9Xx][- ]*){13}|([0-9Xx][- ]*){10})$")]
        public string ISBN { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
