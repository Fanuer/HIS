using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Grants acces to recipe images
    /// </summary>
    public class ImagesController : Controller
    {
        #region CONST

        #endregion

        #region FIELDS

        private readonly IRecipeImageService _service;
        private readonly ILogger<ImagesController> _logger;

        #endregion

        #region CTOR

        /// <summary>
        /// Creates an ImageController, which grants access to images within the recipe management
        /// </summary>
        /// <param name="loggerFactory"></param>
        /// <param name="service"></param>
        public ImagesController(LoggerFactory loggerFactory, IRecipeImageService service)
        {
            _logger = loggerFactory.CreateLogger<ImagesController>();
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
        [HttpGet("api/recipes/{recipeId:int}/images")]
        [ProducesResponseType(typeof(IEnumerable<RecipeImageViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetRecipeImages(int recipeId)
        {
            try
            {
                return Ok(_service.GetImages(recipeId));
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }

        /// <summary>
        /// Returns all images of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="imageId">Id of an image</param>
        /// <returns></returns>
        /// <response code="200">Returns one recipe image</response>
        /// <response code="404">If recipe with the given id is not found</response>
        [HttpGet("api/recipes/{recipeId:int}/images/{imageId:int}")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetRecipeImage(int recipeId, int imageId)
        {
            try
            {
                var result = await _service.GetImage(imageId);
                if (!result.RecipeId.Equals(recipeId))
                {
                    ModelState.AddModelError("recipeId", "The recipe does not contain an image with the given iamge id");
                }
                return Ok(result);
            }
            catch (DataObjectNotFoundException e)
            {
                return NotFound(e.Message);
            }
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}
