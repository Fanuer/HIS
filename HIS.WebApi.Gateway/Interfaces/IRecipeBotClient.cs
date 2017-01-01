using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;

namespace HIS.WebApi.Gateway.Interfaces
{
    public interface IRecipeBotClient
    {
        Task<IEnumerable<ShortRecipeViewModel>> GetRecipes();
        Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId);

        Task<StepViewModel> GetStepAsync(int recipeId, int stepId, StepDirection direction);
        Task StartCookingAsync(int recipeId);
    }
}
