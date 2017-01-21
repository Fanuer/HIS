using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HIS.Gateway.Services.Interfaces;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HIS.Gateway.WebApi.Controllers
{
    /// <summary>
    /// Grants access to recipes 
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class RecipesController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public RecipesController(IGatewayRecipeClient client)
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
        /// <response code="400">If parameters were incorrect</response>
        [ProducesResponseType(typeof(ListViewModel<ShortRecipeViewModel>), (int)HttpStatusCode.OK)]
        [HttpGet]
        public async Task<IActionResult> GetRecipesAsync([FromQuery]RecipeSearchViewModel searchModel, [FromQuery]int page = 0, [FromQuery]int entriesPerPage = 10)
        {
            await Client.SetBearerTokenAsync(this.HttpContext);
            return Ok(await this.Client.GetRecipes(searchModel, page, entriesPerPage));
        }

        /// <summary>
        /// Returns all ingrediants of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        [ProducesResponseType(typeof(IEnumerable<RecipeIngrediantViewModel>), (int)HttpStatusCode.OK)]
        [HttpGet("{recipeId:int}/Ingrediants")]
        public async Task<IActionResult> GetRecipeIngrediantsAsync(int recipeId)
        {
            await Client.SetBearerTokenAsync(this.HttpContext);
            return Ok(await this.Client.GetRecipeIngrediantsAsync(recipeId));
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
        public async Task<IActionResult> GetStepByIdAsync(int recipeId, int stepId, [FromQuery]StepDirection direction = StepDirection.ThisStep)
        {
            await Client.SetBearerTokenAsync(this.HttpContext);
            return Ok(await this.Client.GetStepAsync(recipeId, stepId, direction));
        }

        /// <summary>
        /// Marks a recipe as cooked. This infects the order of recipes shown on a search. 
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        [HttpPost("{recipeId:int}/Cooking")]
        public async Task StartCookingAsync(int recipeId)
        {
            await Client.SetBearerTokenAsync(this.HttpContext);
            await this.Client.StartCookingAsync(recipeId);
        }
        #endregion

        #region PROPERTIES
        public IGatewayRecipeClient Client { get; }
        #endregion 
    }
}
