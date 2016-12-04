using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeSourceRecipe
    {
        [Key, Column(Order = 0)]
        public int RecipeId { get; set; }
        [Key, Column(Order = 1)]
        public int SourceId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual RecipeBaseSource Source { get; set; }

        public int Page{ get; set; }
    }
}
