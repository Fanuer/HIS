using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeTag : INamedEntity<int>
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipeTag()
        {
            Recipes = new HashSet<RecipeRecipeTag>();
        }
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public int Id { get; set; }
        [Required]
        
        public string Name { get; set; }
        public virtual ICollection<RecipeRecipeTag> Recipes { get; set; }

        #endregion
    }
}
