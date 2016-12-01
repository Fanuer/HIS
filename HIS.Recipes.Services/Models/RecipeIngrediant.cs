using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeIngrediant
    {
        [Key, Column(Order = 0)]
        public Guid RecipeId { get; set; }
        [Key, Column(Order = 1)]
        public Guid IngrediantId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingrediant Ingrediant { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public CookingUnit CookingUnit { get; set; }
    }
}
