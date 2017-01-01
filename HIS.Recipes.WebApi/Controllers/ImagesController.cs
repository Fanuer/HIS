using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Grants access to recipe images
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize]
    public class ImagesController : Controller
    {
        #region CONST

        #endregion

        #region FIELDS

        private readonly IRecipeImageService _service;

        #endregion

        #region CTOR

        /// <summary>
        /// Creates an ImageController, which grants access to images within the recipe management
        /// </summary>
        /// <param name="service">service grants access to the image store</param>
        public ImagesController(IRecipeImageService service)
        {
            _service = service;
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Returns all images of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <returns></returns>
        /// <response code="200">Images of a recipe</response>
        /// <response code="404">If recipe with the given id is not found</response>
        [HttpGet("Recipes/{recipeId:int}/Images")]
        [ProducesResponseType(typeof(IEnumerable<RecipeImageViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetRecipeImages(int recipeId)
        {
            return Ok(_service.GetImages(recipeId));
        }

        /// <summary>
        /// Returns all images of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="imageId">Id of an image</param>
        /// <returns></returns>
        /// <response code="200">Returns one recipe image</response>
        /// <response code="404">If recipe with the given id is not found</response>
        [HttpGet("Recipes/{recipeId:int}/Images/{imageId:int}", Name = "GetImageById")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecipeImage(int recipeId, int imageId)
        {
            var result = await _service.GetImage(imageId);
            if (!result.RecipeId.Equals(recipeId))
            {
                ModelState.AddModelError("recipeId", "The recipe does not contain an image with the given image id");
            }
            return Ok(result);
        }

        /// <summary>
        /// Creates a new Image
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="imageData">data of a new image</param>
        /// <returns></returns>
        /// <response code="201">Returns one recipe image</response>
        /// <response code="404">If recipe with the given id is not found</response>
        [HttpPost("Recipes/{recipeId:int}/Images")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateImageAsync(int recipeId, [FromBody]IFormFile imageData)
        {
            var result = await _service.AddAsync(recipeId, imageData);
            return CreatedAtRoute("GetImageById", new { recipeId , imageId = result.Id},  result);
        }

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
        [HttpPut("Recipes/{recipeId:int}/Images/{id:int}")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateImageAsync(int recipeId, int id, [FromBody]IFormFile imageData)
        {
            await _service.UpdateAsync(recipeId, id, imageData);
            return NoContent();
        }

        /// <summary>
        /// Removes an existing image
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Id of the image to remove</param>
        /// <returns></returns>
        [HttpDelete("Recipes/{recipeId:int}/Images/{imageId:int}")]
        public async Task<IActionResult> DeleteImageAsync(int recipeId, int id)
        {
            if (await this._service.GetImages(recipeId).AllAsync(x => !x.Id.Equals(id)))
            {
                return NotFound($"No step with the id {id} found for recipe {recipeId}");
            }

            await _service.RemoveAsync(id);
            return NoContent();
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
