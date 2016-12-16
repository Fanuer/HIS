using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Performs actions on recipes
    /// </summary>
    /// <response code="500">An internal error occurs while performing the action</response>
    //[Authorize]
    [Route("api/[controller]")]
    public class RecipesController : Controller
    {
        #region CONST

        #endregion

        #region FIELDS

        private readonly ILogger<RecipesController> _logger;
        private readonly IRecipeService _service;

        #endregion

        #region CTOR

        /// <summary>
        /// Creates a new RecipesController
        /// </summary>
        /// <param name="loggerFactory">factory to create a logger</param>
        /// <param name="service">service which grants acces to a recipe store</param>
        public RecipesController(ILoggerFactory loggerFactory, IRecipeService service)
        {
            _logger = loggerFactory.CreateLogger<RecipesController>();
            _service = service;
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Get all recipes
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available recipes</response>
        [HttpGet]
        public async Task<IQueryable<ShortRecipeViewModel>> GetRecipesAsync()
        {
            var result = _service.GetRecipes();
            await result.ForEachAsync(x => x.Url = this.Url.RouteUrl("GetRecipeById", new {id = x.Id}));
            return result;
        }

        /// <summary>
        /// Gets information for one recipe
        /// </summary>
        /// <param name="id">Id of the recipe</param>
        /// <returns></returns>
        /// <response code="404">If the no recipe with the given Id is found</response>
        [HttpGet("{id:int}", Name = "GetRecipeById")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRecipeAsync(int id)
        {
            try
            {
                var result = await this._service.GetRecipeAsync(id);
                return Ok(result);
            }
            catch (DataObjectNotFoundException e)
            {
                return this.NotFound(e.Message);
            }
        }

        /// <summary>
        /// Creates a new Recipe
        /// </summary>
        /// <param name="model">Data of the new recipe</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new recipe</response>
        /// <response code="400">If the given data are invalid</response>
        [ProducesResponseType(typeof(FullRecipeViewModel), (int) HttpStatusCode.Created)]
        [HttpPost]
        public async Task<IActionResult> CreateRecipeAsync([FromBody] RecipeCreationViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await _service.AddAsync(model);
                return CreatedAtRoute("GetRecipeById", new {id = result.Id}, result);
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Updates an available recipe
        /// </summary>
        /// <param name="id">Id of the recipe to change</param>
        /// <param name="model">New recipe data</param>
        /// <response code="200">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no recipe was found for the given id</response>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateRecipeAsync(int id, [FromBody] RecipeUpdateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _service.UpdateAsync(id, model);
                return Ok();
            }
            catch (IdsNotIdenticalException e)
            {
                ModelState.AddModelError("id", e.Message);
                return BadRequest(ModelState);
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        /// <summary>
        /// Removes an existing recipe
        /// </summary>
        /// <param name="id">Id of the recipe to delete</param>

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteRecipeAsync(int id)
        {
            try
            {
                await _service.RemoveAsync(id);
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
            return Ok();
        }


        /// <summary>
        /// Flags a recipe as currently cooked
        /// </summary>
        /// <param name="id">id of the recipe</param>
        /// <returns></returns>
        [HttpPost("{id:int}/cooking")]
        public async Task<IActionResult> CookNowAsync(int id)
        {
            try
            {
                await _service.CookNowAsync(id);
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
            return Ok();
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}
