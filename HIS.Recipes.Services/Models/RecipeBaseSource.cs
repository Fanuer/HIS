using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeBaseSource : INamedEntity<Guid>
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipeBaseSource()
        {
            RecipeSourceRecipes = new HashSet<RecipeSourceRecipe>();
        }
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Name { get; set; }

        public virtual ICollection<RecipeSourceRecipe> RecipeSourceRecipes { get; set; }
        #endregion
    }
}
