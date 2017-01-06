using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HIS.Recipes.Services.Models
{
    internal class RecipeNoSource:RecipeBaseSource
    {
        public const string NOSOURCE_NAME = "NO SOURCE";

        public RecipeNoSource()
        {
            Name = NOSOURCE_NAME;
        }
    }
}
