using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HIS.Recipes.WebApi.Controllers
{
    /// <summary>
    /// Grants access to steps of a recipe
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}")]
    [Authorize]
    public class StepsController : Controller
    {

        #region CONST
        #endregion

        #region FIELDS
        private readonly IRecipeStepService _service;
        #endregion

        #region CTOR
        /// <summary>
        /// Creates a new StepsController to access recipe step interaction
        /// </summary>
        /// <param name="service"></param>
        public StepsController(IRecipeStepService service)
        {
            _service = service;
        }
        #endregion

        #region METHODS

        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        [HttpGet("Recipes/{recipeId:int}/Steps")]
        public IQueryable<StepViewModel> GetStepsForRecipe(int recipeId)
        {
            return _service.GetStepsForRecipe(recipeId);
        }

        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="id">Id of a step</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        [HttpGet("Recipes/{recipeId:int}/Steps/{id:int}", Name = "GetStepById")]
        [ProducesResponseType(typeof(StepViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStepsForRecipe(int recipeId, int id)
        {
            return await this.GetStepsForRecipe(recipeId, id, StepDirection.ThisStep);
        }

        /// <summary>
        /// Returns all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of a recipe</param>
        /// <param name="id">Id of a step</param>
        /// <param name="direction">To provide navigation you define if the step of the given id or one of its neighbors</param>
        /// <response code="200">Steps of a Recipe</response>
        /// <response code="404">If no recipe with the given id was found</response>
        [HttpGet("Recipes/{recipeId:int}/Steps/{id:int}/{direction}")]
        [ProducesResponseType(typeof(StepViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetStepsForRecipe(int recipeId, int id, StepDirection direction)
        {
            return Ok(await _service.GetStepAsync(recipeId, id, direction));
        }

        /// <summary>
        /// Creates a new Step
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="model">data of a new step</param>
        /// <returns></returns>
        /// <response code="201">Returns one recipe step</response>
        /// <response code="404">If recipe with the given id is not found</response>
        [HttpPost("Recipes/{recipeId:int}/Steps")]
        [ProducesResponseType(typeof(StepViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateStepAsync(int recipeId, [FromBody]StepCreateViewModel model)
        {
            var result = await _service.AddAsync(recipeId, model);
            return CreatedAtRoute("GetStepById", new { recipeId, id = result.Id }, result);
        }

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
        [HttpPut("Recipes/{recipeId:int}/Steps/{id:int}")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateStepAsync(int recipeId, int id, [FromBody]StepUpdateViewModel model)
        {
            if (await this._service.GetStepsForRecipe(recipeId).AllAsync(x=>!x.Id.Equals(id)))
            {
                return NotFound($"No step with the id {id} found for recipe {recipeId}");
            }
            await _service.UpdateAsync(id, model);
            return NoContent();
        }

        /// <summary>
        /// Updates all steps of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="model">Data of all steps</param>
        /// <returns></returns>
        /// <response code="204">Ater update was successfully</response>
        /// <response code="404">If recipe or step with the given id is not found</response>
        /// <response code="400">If given data were invalid</response>
        [HttpPut("Recipes/{recipeId:int}/Steps")]
        [ProducesResponseType(typeof(RecipeImageViewModel), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateAllStepsAsync(int recipeId,[FromBody]ICollection<string> model)
        {
            await _service.UpdateAllStepsAsync(recipeId, model);
            return NoContent();
        }

        /// <summary>
        /// Removes an existing step
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="id">Id of the step to remove</param>
        /// <returns></returns>
        [HttpDelete("Recipes/{recipeId:int}/Steps/{id:int}")]
        public async Task<IActionResult> DeleteStepAsync(int recipeId, int id)
        {
            if (await this._service.GetStepsForRecipe(recipeId).AllAsync(x => !x.Id.Equals(id)))
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
