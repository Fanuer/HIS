using System.Collections.Generic;
using System.Linq;
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
    /// Grants Access to Sources of Recipes
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize]
    public class SouceController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        private readonly ISourceService _service;
        #endregion

        #region CTOR

        public SouceController(ISourceService service)
        {
            _service = service;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Returns a List of all Sources
        /// </summary>
        /// <response code="200">All Source Entries</response>
        [HttpGet("Sources")]
        [ProducesResponseType(typeof(IEnumerable<SourceListEntryViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetSource()
        {
            return Ok(_service.GetSources());
        }

        /// <summary>
        /// Returns a List of all Cookbooks
        /// </summary>
        /// <response code="200">All Cookbook Entries</response>
        [HttpGet("Cookbooks")]
        [ProducesResponseType(typeof(IEnumerable<CookbookSourceViewModel>), (int)HttpStatusCode.OK)]
        public IActionResult GetCookbooks()
        {
            return Ok(_service.GetCookbooks());
        }

        /// <summary>
        /// Returns one source entry
        /// </summary>
        /// <response code="200">All Cookbook Entries</response>
        /// <response code="404">If the Source is not found</response>
        [HttpGet("Sources/{sourceId:int}", Name = "GetSourceById")]
        public async Task<IActionResult> GetSourceAsync(int sourceId)
        {
            var result = await _service.GetSources().SingleOrDefaultAsync(x => x.Id.Equals(sourceId));
            if (result == null)
            {
                return NotFound($"No source with the given id '{sourceId}'");
            }
            return Ok(result);
        }

        /// <summary>
        /// Creates a new Cookbook
        /// </summary>
        /// <response code="204">After Creation</response>
        [HttpPost("Cookbooks")]
        public async Task<IActionResult> AddCookbookAsync([FromBody]CookbookSourceCreationViewModel model)
        {
            var result = await _service.AddCookbookAsync(model);
            return CreatedAtRoute("GetSourceById", new {sourceId = result.Id}, result); 
        }

        /// <summary>
        /// Creates a new WebSource for a recipe
        /// </summary>
        [ProducesResponseType(typeof(WebSourceViewModel), (int)HttpStatusCode.Created)]
        [HttpPost("api/Recipes/{recipeId:int}/WebSource")]
        public async Task<IActionResult> AddWebSourceAsync(int recipeId, [FromBody]WebSourceCreationViewModel model)
        {
            var result = await _service.AddWebSourceAsync(recipeId, model);
            return CreatedAtRoute("GetSourceById", new { sourceId = result.Id }, result);
        }

        /// <summary>
        /// Removes a Source
        /// </summary>
        /// <returns></returns>
        [HttpDelete("Sources/{sourceId:int}")]
        public async Task<IActionResult> RemoveSourceAsync(int sourceId)
        {
            await _service.RemoveSourceAsync(sourceId);
            return NoContent();
        }

        /// <summary>
        /// Updates a Cookbook
        /// </summary>
        /// <returns></returns>
        [HttpPut("Cookbooks/{sourceId:int}")]
        public async Task<IActionResult> UpdateCookbookAsync(int sourceId, [FromBody]CookbookSourceViewModel model)
        {
            await _service.UpdateCookbookAsync(sourceId, model);
            return NoContent();
        }

        /// <summary>
        /// Updates a WebSource
        /// </summary>
        /// <returns></returns>
        [HttpPut("Recipes/{recipeId:int}/WebSource/{sourceId:int}")]
        public async Task<IActionResult> UpdateWebSourceAsync(int recipeId, int sourceId, [FromBody]WebSourceViewModel model)
        {
            await _service.UpdateWebSourceAsync(recipeId, sourceId, model);
            return NoContent();
        }

        /// <summary>
        /// Updates a Cookbook source of a recipe
        /// </summary>
        /// <returns></returns>
        [HttpPut("Recipes/{recipeId:int}/Cookbook/{sourceId:int}/{page:int}")]
        public async Task<IActionResult> UpdateRecipeCookbookSourceAsync(int recipeId, int sourceId, int page)
        {
            await _service.UpdateRecipeOnCookbookAsync(recipeId, sourceId, page);
            return NoContent();
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
