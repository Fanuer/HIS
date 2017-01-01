using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Grants access to ingrediants within the recipe management
    /// </summary>
    [Authorize]
    [ApiVersion("1.0")]
    public class IngrediantsController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        private readonly IIngrediantService _service;

        #endregion

        #region CTOR
        /// <summary>
        /// Creates a new Tag Controller, which grants access to interaction with ingrediants
        /// </summary>
        /// <param name="service">service grants acces to the ingrediant store</param>
        public IngrediantsController(IIngrediantService service)
        {
            _service = service;
        }
        #endregion

        #region METHODS
        /// <summary>
        /// Returns all tags
        /// </summary>
        /// <returns></returns>
        /// <response code="200">Returns a list of all available tags</response>
        [HttpGet("api/v{version:apiVersion}/[controller]")]
        public async Task<IQueryable<IngrediantStatisticViewModel>> GetIngrediants()
        {
            var result = this._service.GetIngrediantList();
            await result.ForEachAsync(x => x.Url = this.Url.RouteUrl("GetIngrediantById", new { id = x.Id }));
            return result;
        }

        /// <summary>
        /// Returns one ingrediant
        /// </summary>
        /// <param name="id">Id of an ingrediant</param>
        /// <response code="404">If the no ingrediant with the given Id is found</response>
        [HttpGet("api/v{version:apiVersion}/[controller]/{id:int}", Name = "GetIngrediantById")]
        public async Task<IActionResult> GetIngrediantAsync(int id)
        {
            var tag = await this._service.GetIngrediantList().SingleOrDefaultAsync(x => x.Id.Equals(id));

          if (tag == null) return NotFound("No Tag with the given id found");
          tag.Url = this.Url.RouteUrl("GetIngrediantById", new {id = tag.Id});
          return Ok(tag);
        }

        /// <summary>
        /// Creates a new ingrediant
        /// </summary>
        /// <param name="model">Data of the new Ingrediant</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new Ingrediant</response>
        /// <response code="400">If the given data are invalid</response>
        [ProducesResponseType(typeof(NamedViewModel), (int)HttpStatusCode.Created)]
        [HttpPost("api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> CreateIngrediantAsync([FromBody] string model)
        {
            var result = await _service.AddAsync(model);
            result.Url = this.Url.RouteUrl("GetIngrediantById", new { id = result.Id });
            return CreatedAtRoute("GetIngrediantById", new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an available ingrediant
        /// </summary>
        /// <param name="id">Id of the ingrediant to change</param>
        /// <param name="model">New tag data</param>
        /// <response code="204">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no tag was found for the given id</response>
        [HttpPut("api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> UpdateIngrediantAsync(int id, [FromBody]NamedViewModel model)
        {
            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Removes an existing ingrediant
        /// </summary>
        /// <param name="id">Id of the tag to delete</param>
        /// <response code="204">After deletion</response>
        [HttpDelete("api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> DeleteIngrediantAsync(int id)
        {
            await _service.RemoveAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Returns all ingrediants of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        [HttpGet("api/v{version:apiVersion}/Recipes/{recipeId:int}/Ingrediants")]
        [ProducesResponseType(typeof(IQueryable<IngrediantViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetIngrediantsForRecipe(int recipeId)
        {
            return Ok(_service.GetIngrediantsForRecipe(recipeId));
        }

        /// <summary>
        /// Updates an ingrediant of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="model">New Data of an ingrediant</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        [HttpPut, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Ingrediants")]
        public async Task<IActionResult> AddIngrediantToRecipeAsync(int recipeId, [FromBody]AlterIngrediantViewModel model)
        {
            await _service.AddOrUpdateIngrediantToRecipeAsync(model);
            return Ok();
        }

        /// <summary>
        /// Removes an ingrediant from a recipe
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="ingrediantId">Id of a Ingrediant</param>
        /// <response code="200">After removing was successfully</response>
        /// <response code="404">If Recipe or ingrediant was not found by the given id</response>
        [HttpDelete, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Tags/{ingrediantId:int}")]
        public async Task<IActionResult> RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId)
        {
            await _service.RemoveIngrediantFromRecipeAsync(recipeId, ingrediantId);
            return Ok();
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
