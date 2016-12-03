using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class Recipe : INamedEntity<Guid>
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        internal Recipe()
        {
            this.LastTimeCooked = new DateTime();
            this.Images = new HashSet<RecipeImage>();
            this.Steps = new HashSet<RecipeStep>();
            this.Ingrediants = new HashSet<RecipeIngrediant>();
            this.Tags = new HashSet<RecipeRecipeTag>();
            this.Source = new RecipeSourceRecipe();
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
        [Required]
        public int NumberOfServings { get; set; }
        public int Calories { get; set; }
        public string Creator { get; set; }
        public int CookedCounter { get; set; }
        [Required]
        public DateTime LastTimeCooked { get; set; }
        public virtual ICollection<RecipeRecipeTag> Tags { get; set; }

        public virtual ICollection<RecipeIngrediant> Ingrediants { get; set; }

        public virtual ICollection<RecipeImage> Images { get; set; }

        public virtual RecipeSourceRecipe Source { get; set; }

        public Guid SourceId { get; set; }
        public virtual ICollection<RecipeStep> Steps { get; set; }

        #endregion
    }
}
