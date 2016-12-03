using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Services.Models;
using Xunit;

namespace HIS.Recipes.Services.Tests.EntityTests
{
    public class RecipeSourceTests
    {
        [Fact]
        public void ReturnRightEnumForType()
        {
            var webSource = new RecipeUrlSource();
            var output = webSource.GetSourceType();
            Assert.Equal(SourceType.WebSource, output);

            var cookbook = new RecipeCookbookSource();
            output = cookbook.GetSourceType();
            Assert.Equal(SourceType.Cookbook, output);

            var invalidSource = new InvalidSource();
            Assert.Throws(typeof(ArgumentException), () => invalidSource.GetSourceType());
        }

        #region NESTED

        private class InvalidSource : RecipeBaseSource
        {
            
        }

        #endregion
    }
}
