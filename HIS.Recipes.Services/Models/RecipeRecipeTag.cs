using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeRecipeTag
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipeRecipeTag()
        {
            
        }

        public RecipeRecipeTag(Recipe recipe, RecipeTag tag)
        {
            if (recipe == null) { throw new ArgumentNullException(nameof(recipe)); }
            if (tag== null) { throw new ArgumentNullException(nameof(tag)); }

            this.Recipe = recipe;
            this.RecipeId = recipe.Id;
            this.RecipeTag = tag;
            this.RecipeTagId = tag.Id;
        }
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        [Key, Column(Order = 0)]
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }

        [Key, Column(Order = 1)]
        public Guid RecipeTagId { get; set; }
        public RecipeTag RecipeTag { get; set; }
        #endregion
    }
}
