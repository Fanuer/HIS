using System;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeStep:IEntity<Guid>
    {
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int Order { get; set; }
        public Guid RecipeId { get; set; }
        public Recipe Recipe { get; set; }
    }
}
