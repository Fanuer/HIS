using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Grants access to tags within the recipe management
    /// </summary>
    [ApiVersion("1.0")]
    public class TagsController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        private readonly ITagService _service;
        #endregion

        #region CTOR
        /// <summary>
        /// Creates a new Tag Controller, which grants access to interaction with tags
        /// </summary>
        /// <param name="service">service grants acces to the tag store</param>
        public TagsController(ITagService service)
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
        public async Task<IQueryable<NamedViewModel>> GetTags()
        {
            var result = this._service.GetTags();
            await result.ForEachAsync(x => x.Url = this.Url.RouteUrl("GetTagById", new {id = x.Id}));
            return result;
        }

        /// <summary>
        /// Returns one tag
        /// </summary>
        /// <param name="id">Id of the Tag</param>
        /// <response code="404">If the no tag with the given Id is found</response>
        [HttpGet("api/v{version:apiVersion}/[controller]/{id:int}", Name = "GetTagById")]
        public async Task<IActionResult> GetTagsAsync(int id)
        {
            var tag = await _service.GetTags().SingleOrDefaultAsync(x => x.Id.Equals(id));
            if (tag != null)
            {
                return Ok(tag);
            }

            return NotFound("No Tag with the given id found");
        }

        /// <summary>
        /// Creates a new Tag
        /// </summary>
        /// <param name="model">Data of the new tag</param>
        /// <returns></returns>
        /// <response code="201">After Creation of the new tag</response>
        /// <response code="400">If the given data are invalid</response>
        [ProducesResponseType(typeof(NamedViewModel), (int)HttpStatusCode.Created)]
        [HttpPost("api/v{version:apiVersion}/[controller]")]
        public async Task<IActionResult> CreateRecipeAsync([FromBody] string model)
        {
            var result = await _service.AddAsync(model);
            result.Url = this.Url.RouteUrl("GetTagById", new {id = result.Id});
            return CreatedAtRoute("GetTagById", new { id = result.Id }, result);
        }

        /// <summary>
        /// Updates an available tag
        /// </summary>
        /// <param name="id">Id of the tag to change</param>
        /// <param name="model">New tag data</param>
        /// <response code="204">After update was successfully</response>
        /// <response code="400">If the given data are invalid</response>
        /// <response code="404">If no tag was found for the given id</response>
        [HttpPut("api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> UpdateRecipeAsync(int id, [FromBody]NamedViewModel  model)
        {
            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Removes an existing tag
        /// </summary>
        /// <param name="id">Id of the tag to delete</param>
        /// <response code="204">After deletion</response>
        [HttpDelete("api/v{version:apiVersion}/[controller]/{id:int}")]
        public async Task<IActionResult> DeleteRecipeAsync(int id)
        {
            await _service.RemoveAsync(id);
            return NoContent();
        }

        /// <summary>
        /// Adds a Tag to a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagId">Id of an existing Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Tag or Recipe was not found by the given id</response>
        [HttpPost, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Tags/{tagId:int}")]
        public async Task<IActionResult> AddTagToRecipeAsync(int recipeId, int tagId)
        {
            await _service.AddTagToRecipeAsync(recipeId, tagId);
            return Ok();
        }

        /// <summary>
        /// Adds a Tag to a Recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagName">Name of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        [HttpPost, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Tags/{tagName}")]
        public async Task<IActionResult> AddTagToRecipeAsync(int recipeId, string tagName)
        {
            if (String.IsNullOrWhiteSpace(tagName))
            {
                ModelState.AddModelError("tagName", "Tag name must not be null or empty");
                return BadRequest(ModelState);
            }
            await _service.AddTagToRecipeAsync(recipeId, tagName);
            return Ok();
        }

        /// <summary>
        /// Removes a Tag from a recipe
        /// </summary>
        /// <param name="recipeId">Id of a Recipe</param>
        /// <param name="tagId">Id of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        [HttpDelete, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Tags/{tagId:int}")]
        public async Task<IActionResult> RemoveTagFromRecipeAsync(int recipeId, int tagId)
        {
            await _service.RemoveTagFromRecipeAsync(recipeId, tagId);
            return Ok();
        }

        /// <summary>
        /// Removes a Tag from a recipe
        /// </summary>
        /// <param name="recipeId">Id of the Recipe</param>
        /// <param name="tagName">Name of a Tag</param>
        /// <response code="200">After adding was successfully</response>
        /// <response code="404">If Recipe was not found by the given id</response>
        [HttpDelete, Route("api/v{version:apiVersion}/Recipes/{recipeId:int}/Tags/{tagName}")]
        public async Task<IActionResult> RemoveTagFromRecipeAsync(int recipeId, string tagName)
        {
            if (String.IsNullOrWhiteSpace(tagName))
            {
                ModelState.AddModelError("tagName", "Tag name must not be null or empty");
                return BadRequest(ModelState);
            }
            await _service.RemoveTagFromRecipeAsync(recipeId, tagName);
            return Ok();
        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
