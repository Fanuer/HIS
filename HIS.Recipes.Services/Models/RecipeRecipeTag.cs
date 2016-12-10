using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Models
{
    [Table("RecipeRecipeTags")]
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

            this.RecipeId = recipe.Id;
            this.RecipeTagId = tag.Id;
        }

        public RecipeRecipeTag(int recipeId, int tagId)
        {
            if (recipeId == default(int)) { throw new ArgumentNullException(nameof(recipeId)); }
            if (tagId == default(int)) { throw new ArgumentNullException(nameof(tagId)); }

            this.RecipeId = recipeId;
            this.RecipeTagId = tagId;

        }
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
        public int RecipeTagId { get; set; }
        public RecipeTag RecipeTag { get; set; }
        #endregion
    }
}
