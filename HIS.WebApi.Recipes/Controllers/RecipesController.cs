using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.WebApi.Controllers
{
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

        public RecipesController(ILoggerFactory loggerFactory, IRecipeService service)
        {
            _logger = loggerFactory.CreateLogger<RecipesController>();
            _service = service;
        }
        #endregion

        #region METHODS

        // GET api/values
        [HttpGet]
        public IQueryable<ShortRecipeViewModel> Get()
        {
            var result =  _service.GetRecipes();
            return result;
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        #endregion

        #region PROPERTIES
        #endregion


    }
}
