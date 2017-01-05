using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIS.Gateway.Services.Interfaces
{
    public interface IRecipeClient : IS2SClient
    {
        #region Images
        /// <summary>
        /// Returns all images of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <returns></returns>
        /// <response code="200">Images of a recipe</response>
        /// <response code="404">If recipe with the given id is not found</response>
        IActionResult GetRecipeImages(int recipeId);
        /// <summary>
        /// Returns all images of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="imageId">Id of an image</param>
        /// <returns></returns>
        /// <response code="200">Returns one recipe image</response>
        /// <response code="404">If recipe with the given id is not found</response>
        Task<IActionResult> GetRecipeImage(int recipeId, int imageId);
        /// <summary>
        /// Creates a new Image
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="imageData">data of a new image</param>
        /// <returns></returns>
        /// <response code="201">Returns one recipe image</response>
        /// <response code="404">If recipe with the given id is not found</response>
        Task<IActionResult> CreateImageAsync(int recipeId, IFormFile imageData);
        /// <summary>
        /// Updates an existing Image
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Image id</param>
        /// <param name="imageData">Data of an image</param>
        /// <returns></returns>
        /// <response code="204">Ater update was successfully</response>
        /// <response code="404">If recipe or image with the given id is not found</response>
        /// <response code="400">If given data were invalid</response>
        Task<IActionResult> UpdateImageAsync(int recipeId, int id, IFormFile imageData);
        /// <summary>
        /// Removes an existing image
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Id of the image to remove</param>
        /// <returns></returns>
        Task<IActionResult> DeleteImageAsync(int recipeId, int id);

        #endregion
        
        #region Ingrediants
        /// <summary>
        /// Returns all tags
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available tags</response>
        Task<IQueryable<IngrediantStatisticViewModel>> GetIngrediants();

        /// <summary>
        /// Returns one ingrediant
        /// </summary>
        /// <param name="id">Id of an ingrediant</param>
        /// <response code="404">If the no ingrediant with the given Id is found</response>
        Task<IActionResult> GetIngrediantAsync(int id);

        /// <summary>
        /// Creates a new ingrediant
        /// </summary>
        /// <param name="model">Data of the new Ingrediant</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new Ingrediant</response>
        /// <response code="400">If the given data are invalid</response>
        Task<IActionResult> CreateIngrediantAsync( string model);

        /// <summary>
        /// Updates an available ingrediant
        /// </summary>
        /// <param name="id">Id of the ingrediant to change</param>
        /// <param name="model">New tag data</param>
        /// <response code="204">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no tag was found for the given id</response>
        Task<IActionResult> UpdateIngrediantAsync(int id, NamedViewModel model);

        /// <summary>
        /// Removes an existing ingrediant
        /// </summary>
        /// <param name="id">Id of the tag to delete</param>
        /// <response code="204">After deletion</response>
        Task<IActionResult> DeleteIngrediantAsync(int id);

        /// <summary>
        /// Returns all ingrediants of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        IActionResult GetIngrediantsForRecipe(int recipeId);

        /// <summary>
        /// Updates an ingrediant of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="model">New Data of an ingrediant</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        Task<IActionResult> AddIngrediantToRecipeAsync(int recipeId, AlterIngrediantViewModel model);

        /// <summary>
        /// Removes an ingrediant from a recipe
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="ingrediantId">Id of a Ingrediant</param>
        /// <response code="200">After removing was successfully</response>
        /// <response code="404">If Recipe or ingrediant was not found by the given id</response>
        Task<IActionResult> RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId);
        #endregion
        
        #region Recipes
        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available recipes</response>
        Task<IQueryable<ShortRecipeViewModel>> GetRecipesAsync();

        /// <summary>
        /// Gets information for one recipe
        /// </summary>
        /// <param name="id">Id of the recipe</param>
        /// <returns></returns>
        /// <response code="404">If the no recipe with the given Id is found</response>
        Task<IActionResult> GetRecipeAsync(int id);

        /// <summary>
        /// Creates a new Recipe
        /// </summary>
        /// <param name="model">Data of the new recipe</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new recipe</response>
        /// <response code="400">If the given data are invalid</response>
        Task<IActionResult> CreateRecipeAsync( RecipeCreationViewModel model);

        /// <summary>
        /// Updates an available recipe
        /// </summary>
        /// <param name="id">Id of the recipe to change</param>
        /// <param name="model">New recipe data</param>
        /// <response code="200">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no recipe was found for the given id</response>
        Task<IActionResult> UpdateRecipeAsync(int id,  RecipeUpdateViewModel model);

        /// <summary>
        /// Removes an existing recipe
        /// </summary>
        /// <param name="id">Id of the recipe to delete</param>
        Task<IActionResult> DeleteRecipeAsync(int id);

        /// <summary>
        /// Flags a recipe as currently cooked
        /// </summary>
        /// <param name="id">id of the recipe</param>
        /// <returns></returns>
        
        Task<IActionResult> CookNowAsync(int id);

        #endregion
        
        #region Sources
        /// <summary>
        /// Returns a List of all Sources
        /// </summary>
        /// <response code="200">All Source Entries</response>
        IActionResult GetSource();

        /// <summary>
        /// Returns a List of all Cookbooks
        /// </summary>
        /// <response code="200">All Cookbook Entries</response>
        IActionResult GetCookbooks();

        /// <summary>
        /// Returns one source entry
        /// </summary>
        /// <response code="200">All Cookbook Entries</response>
        /// <response code="404">If the Source is not found</response>
        Task<IActionResult> GetSourceAsync(int sourceId);

        /// <summary>
        /// Creates a new Cookbook
        /// </summary>
        /// <response code="204">After Creation</response>
        Task<IActionResult> AddCookbookAsync(CookbookSourceCreationViewModel model);

        /// <summary>
        /// Creates a new WebSource for a recipe
        /// </summary>
        Task<IActionResult> AddWebSourceAsync(int recipeId, WebSourceCreationViewModel model);

        /// <summary>
        /// Removes a Source
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> RemoveSourceAsync(int sourceId);

        /// <summary>
        /// Updates a Cookbook
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> UpdateCookbookAsync(int sourceId, CookbookSourceViewModel model);

        /// <summary>
        /// Updates a WebSource
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> UpdateWebSourceAsync(int recipeId, int sourceId, WebSourceViewModel model);

        /// <summary>
        /// Updates a Cookbook source of a recipe
        /// </summary>
        /// <returns></returns>
        Task<IActionResult> UpdateRecipeCookbookSourceAsync(int recipeId, int sourceId, int page);

        #endregion

        #region Steps
        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        IQueryable<StepViewModel> GetStepsForRecipe(int recipeId);


        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="id">Id of a step</param>
        /// <param name="direction">To provide navigation you define if the step of the given id or one of its neighbors</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        Task<IActionResult> GetStepsForRecipe(int recipeId, int id, StepDirection direction = StepDirection.ThisStep);

        /// <summary>
        /// Creates a new Step
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="model">data of a new step</param>
        /// <returns></returns>
        /// <response code="201">Returns one recipe step</response>
        /// <response code="404">If recipe with the given id is not found</response>
        Task<IActionResult> CreateStepAsync(int recipeId, StepCreateViewModel model);

        /// <summary>
        /// Updates an existing step
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Step id</param>
        /// <param name="model">Data of a Step</param>
        /// <returns></returns>
        /// <response code="204">Ater update was successfully</response>
        /// <response code="404">If recipe or step with the given id is not found</response>
        /// <response code="400">If given data were invalid</response>
        Task<IActionResult> UpdateStepAsync(int recipeId, int id, StepUpdateViewModel model);

        /// <summary>
        /// Updates all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="model">Data of all steps</param>
        /// <returns></returns>
        /// <response code="204">Ater update was successfully</response>
        /// <response code="404">If recipe or step with the given id is not found</response>
        /// <response code="400">If given data were invalid</response>
        Task<IActionResult> UpdateAllStepsAsync(int recipeId, ICollection<string> model);

        /// <summary>
        /// Removes an existing step
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Id of the step to remove</param>
        /// <returns></returns>
        Task<IActionResult> DeleteStepAsync(int recipeId, int id);

        #endregion

        #region Tags
        /// <summary>
        /// Returns all tags
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available tags</response>
        Task<IQueryable<NamedViewModel>> GetTags();

        /// <summary>
        /// Returns one tag
        /// </summary>
        /// <param name="id">Id of the Tag</param>
        /// <response code="404">If the no tag with the given Id is found</response>
        Task<IActionResult> GetTagsAsync(int id);

        /// <summary>
        /// Creates a new Tag
        /// </summary>
        /// <param name="model">Data of the new tag</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new tag</response>
        /// <response code="400">If the given data are invalid</response>
        Task<IActionResult> CreateTagAsync( string model);

        /// <summary>
        /// Updates an available tag
        /// </summary>
        /// <param name="id">Id of the tag to change</param>
        /// <param name="model">New tag data</param>
        /// <response code="204">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no tag was found for the given id</response>
        Task<IActionResult> UpdateTagAsync(int id, NamedViewModel model);

        /// <summary>
        /// Removes an existing tag
        /// </summary>
        /// <param name="id">Id of the tag to delete</param>
        /// <response code="204">After deletion</response>
        Task<IActionResult> DeleteTagAsync(int id);

        /// <summary>
        /// Adds a Tag to a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagId">Id of an existing Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        Task<IActionResult> AddTagToRecipeAsync(int recipeId, int tagId);

        /// <summary>
        /// Adds a Tag to a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagName">Name of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        Task<IActionResult> AddTagToRecipeAsync(int recipeId, string tagName);

        /// <summary>
        /// Removes a Tag from a recipe
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="tagId">Id of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        Task<IActionResult> RemoveTagFromRecipeAsync(int recipeId, int tagId);

        /// <summary>
        /// Removes a Tag from a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagName">Name of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        Task<IActionResult> RemoveTagFromRecipeAsync(int recipeId, string tagName);

        #endregion
    }
}
