using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Models.ViewModels
{
    public class WebSourceViewModel:WebSourceCreationViewModel, IViewModelEntity<Guid>
    {
        [Required]
        public Guid Id { get; }
        public string Url { get; }
    }
}
