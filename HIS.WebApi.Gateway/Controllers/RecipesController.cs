using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.WebApi.Gateway.Interfaces;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HIS.WebApi.Gateway.Controllers
{
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RecipesController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipesController(IRecipeBotClient client)
        {
            Client = client;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available recipes</response>
        [HttpGet]
        public async Task<IEnumerable<ShortRecipeViewModel>> GetRecipesAsync()
        {
            return await this.Client.GetRecipes();
        }

        /// <summary>
        /// Returns all ingrediants of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        [HttpGet("{recipeId:int}/Ingrediants")]
        public async Task<IEnumerable<RecipeIngrediantViewModel>> GetRecipeIngrediantsAsync(int recipeId)
        {
            return await this.Client.GetRecipeIngrediantsAsync(recipeId);
        }

        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="stepId">Id of a step</param>
        /// <param name="direction">To provide navigation you define if the step of the given id or one of its neighbors</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        [HttpGet("{recipeId:int}/Steps/{stepId:int}")]
        public async Task<StepViewModel> GetStepByIdAsync(int recipeId, int stepId, StepDirection direction = StepDirection.ThisStep)
        {
            return await this.Client.GetStepAsync(recipeId, stepId, direction);
        }

        #endregion

        #region PROPERTIES
        public IRecipeBotClient Client { get; }
        #endregion 
    }
}
