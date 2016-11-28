using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class TagViewModel:IViewModelEntity<Guid>
    {
        [Required]
        public Guid Id { get; }
        [Required]
        public string Url { get; }
        [Required]
        public string Name { get; set; }
    }
}
