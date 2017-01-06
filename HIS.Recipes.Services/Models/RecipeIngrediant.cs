using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Recipes.Models.Enums;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeIngrediant
    {
        public RecipeIngrediant()
        {
            CookingUnit = CookingUnit.None;
        }

        public RecipeIngrediant(int recipeId, int ingrediantId, double? amount = null, CookingUnit cookingUnit = CookingUnit.None)
        {
            RecipeId = recipeId;
            IngrediantId = ingrediantId;
            Amount = amount;
            CookingUnit = cookingUnit;
        }

        public int RecipeId { get; set; }
        public int IngrediantId { get; set; }

        public virtual Recipe Recipe { get; set; }
        public virtual Ingrediant Ingrediant { get; set; }
        public double? Amount { get; set; }
        [Required]
        public CookingUnit CookingUnit { get; set; }
    }
}
