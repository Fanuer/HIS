using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeIngrediant
    {
        public int RecipeId { get; set; }
        public int IngrediantId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingrediant Ingrediant { get; set; }
        [Required]
        public int Amount { get; set; }
        [Required]
        public CookingUnit CookingUnit { get; set; }
    }
}
