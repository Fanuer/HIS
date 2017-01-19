using System.Collections.Generic;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;

namespace HIS.Gateway.Services.Interfaces
{
    public interface IRecipeBotClient: IS2SClient
    {
        Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId);

        Task<StepViewModel> GetStepAsync(int recipeId, int stepId, StepDirection direction);
        Task StartCookingAsync(int recipeId);
        Task<ListViewModel<ShortRecipeViewModel>> GetRecipes(RecipeSearchViewModel searchModel, int page, int entriesPerPage);
    }
}
