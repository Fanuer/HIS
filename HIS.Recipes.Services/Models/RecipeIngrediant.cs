using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Recipes.Services.Enums;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeIngrediant
    {
        [Key, Column(Order = 0)]
        public Guid RecipeId { get; set; }
        [Key, Column(Order = 1)]
        public Guid IngrediantId { get; set; }

        public virtual Recipe Member { get; set; }
        public virtual Ingrediant Comment { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public CookingUnit CookingUnit { get; set; }
    }
}
