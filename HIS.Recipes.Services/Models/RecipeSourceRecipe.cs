using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeSourceRecipe
    {
        [Key, Column(Order = 0)]
        public Guid RecipeId { get; set; }
        [Key, Column(Order = 1)]
        public Guid SourceId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual RecipeBaseSource Source { get; set; }

        public int? Page{ get; set; }
    }
}
