using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    /// <summary>
    /// Returns an entity with its name, id and url
    /// </summary>
    public class NamedViewModel:IViewModelEntity<Guid>
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
