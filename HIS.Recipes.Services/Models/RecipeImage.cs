using System;
using System.ComponentModel.DataAnnotations;
using HIS.Data.Base.Interfaces.Models;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeImage:IEntity<Guid>
    {
        /// <summary>
        /// DB Key
        /// </summary>
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Url { get; set; }
        [Required]
        public string Filename { get; set; }
        public Guid RecipeId { get; set; }
        public virtual Recipe Recipe { get; set; }
    }
}
