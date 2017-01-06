using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class Recipe : INamedEntity<int>
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

        public Recipe(string name, int numberOfServings, int? calories=null, string creator = "Stefan")
            :this()
        {
            Name = name;
            NumberOfServings = numberOfServings;
            Calories = calories;
            Creator = creator;
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
        [Required]
        public int NumberOfServings { get; set; }
        public int? Calories { get; set; }
        [Required]
        public string Creator { get; set; }
        public int CookedCounter { get; set; }
        [Required]
        public DateTime LastTimeCooked { get; set; }
        public virtual ICollection<RecipeRecipeTag> Tags { get; set; }

        public virtual ICollection<RecipeIngrediant> Ingrediants { get; set; }

        public virtual ICollection<RecipeImage> Images { get; set; }

        public virtual RecipeSourceRecipe Source { get; set; }

        public int SourceId { get; set; }
        public virtual ICollection<RecipeStep> Steps { get; set; }

        #endregion
    }
}
