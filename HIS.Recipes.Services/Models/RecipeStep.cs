using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeStep:IEntity<int>
    {
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Order { get; set; }
        public int RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
