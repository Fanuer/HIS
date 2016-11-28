using System;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class Ingrediant: INamedEntity<Guid>
    {
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the Ingrediant
        /// </summary>
        public string Name { get; set; }
        
    }
}
