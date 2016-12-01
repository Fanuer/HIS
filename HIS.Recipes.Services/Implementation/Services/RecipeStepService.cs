using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Implementation.Services
{
    /// <summary>
    /// Grants acces to recipe steps
    /// </summary>
    internal class RecipeStepService : BaseService<IStepRepository, RecipeStep, StepViewModel, StepCreateViewModel>, IRecipeStepService
    {
        #region CONST

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        public RecipeStepService(IStepRepository rep, IMapper mapper, ILoggerFactory loggerFactory)
            : base(rep, mapper, loggerFactory?.CreateLogger<RecipeStepService>(), "recipe step")
        {
            this.Repository = rep;
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Returns a Steps of a given recipe
        /// </summary>
        /// <param name="recipeId"></param>
        /// <returns></returns>
        public IQueryable<StepViewModel> GetStepsForRecipe(Guid recipeId)
        {
            IQueryable<StepViewModel> result = null;
            try
            {
                result = this.Repository
                    .GetAll()
                    .Where(x => x.RecipeId.Equals(recipeId))
                    .OrderBy(x => x.Order)
                    .ProjectTo<StepViewModel>();

                this.Logger.LogDebug(new EventId(), $"Returned all steps of recipe {recipeId}");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving steps for recipe {recipeId}");
                throw new Exception($"Error on receiving steps for recipe {recipeId}");
            }
            return result;
        }

        #endregion

        #region PROPERTIES

        protected override IStepRepository Repository { get; set; }

        #endregion

    }
}