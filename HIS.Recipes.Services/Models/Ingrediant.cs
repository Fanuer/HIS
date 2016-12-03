using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class Ingrediant: INamedEntity<Guid>
    {
        public Ingrediant()
        {
            RecipeIngrediants = new HashSet<RecipeIngrediant>();
        }

        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// Name of the Ingrediant
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A list of all recipes
        /// </summary>
        public virtual ICollection<RecipeIngrediant> RecipeIngrediants { get; set; }
    }
}
