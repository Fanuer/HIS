﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.Enums;
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
    internal class RecipeStepService : IRecipeStepService
    {
        #region CONST

        #endregion

        #region FIELDS
        bool _disposed;
        private IRecipeRepository _recipeRep;

        #endregion

        #region CTOR

        public RecipeStepService(IStepRepository rep, IRecipeRepository recipeRep, IMapper mapper, ILoggerFactory loggerFactory)
        {
            if (rep == null)
            {
                throw new ArgumentNullException(nameof(rep));
            }
            if (recipeRep == null)
            {
                throw new ArgumentNullException(nameof(recipeRep));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }

            this._recipeRep = recipeRep;
            this.Logger = loggerFactory?.CreateLogger<RecipeStepService>();
            this.Mapper = mapper;
            this.Repository = rep;
        }

        ~RecipeStepService()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Returns a Steps of a given recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <returns></returns>
        public IQueryable<StepViewModel> GetStepsForRecipe(int recipeId)
        {
            IQueryable<StepViewModel> result = null;
            try
            {
                result = this.Repository
                    .GetAll()
                    .Where(x => x.RecipeId.Equals(recipeId))
                    .Include(x=>x.Recipe).ThenInclude(x=>x.Steps)
                    .OrderBy(x => x.Order)
                    .ProjectTo<StepViewModel>(this.Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all steps of recipe {recipeId}");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving steps for recipe {recipeId}");
                throw new Exception($"Error on receiving steps for recipe {recipeId}");
            }
            return result;
        }

        /// <summary>
        /// Searches a step of a recipe
        /// </summary>
        /// <param name="recipeId">Id of the owning Recipe</param>
        /// <param name="stepId">Id of a step</param>
        /// <param name="direction">To provide navigation you define if the step of the given id or one of its neighbors</param>
        /// <returns>Returns one step of a recipe</returns>
        public async Task<StepViewModel> GetStepAsync(int recipeId, int stepId = -1, StepDirection direction = StepDirection.ThisStep)
        {
            StepViewModel result = null;
            var steps = await this.GetStepsForRecipe(recipeId).ToListAsync();

            var step = stepId == -1 ? steps.FirstOrDefault() : steps.SingleOrDefault(x => x.Id.Equals(stepId));
            if (step == null)
            {
                var message = stepId == -1 ? $"Recipe {recipeId} does not define any steps" : $"No step with the id {stepId} was found";
                throw new DataObjectNotFoundException(message);
            }
            var stepIndex = steps.IndexOf(step);
            switch (direction)
            {
                case StepDirection.ThisStep:
                    result = step;
                    break;
                case StepDirection.Next:
                    result = steps.ElementAtOrDefault(stepIndex +1) ?? steps.Last(); 
                    break;
                case StepDirection.Previous:
                    result = steps.ElementAtOrDefault(stepIndex -1) ?? steps.First();
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }
            return result;
        }


        public async Task<StepViewModel> AddAsync(int recipeId, StepCreateViewModel creationModel)
        {
            StepViewModel result = null;
            try
            {
                if (recipeId == default(int)) { throw new ArgumentNullException(nameof(recipeId), "RecipeId must be set"); }
                if (creationModel == null) { throw new ArgumentNullException(nameof(creationModel), "Data to create a step must be set"); }
                
                if (creationModel.Order <= 0)
                {
                    var recipe = await this._recipeRep.FindAsync(recipeId, x => x.Steps);
                    if (recipe == null)
                    {
                        var message = $"No recipe with thie given id '{recipeId}' found";
                        this.Logger.LogError(String.Concat("Creating Step: ", message));
                        throw new DataObjectNotFoundException(message);
                    }
                    creationModel.Order = recipe.Steps.Count;
                    Logger.LogDebug($"No Order defined for new step. Set to max for recipe {recipe.Name}({recipe.Id})");
                }

                var dbModel = this.Mapper.Map<RecipeStep>(creationModel);
                dbModel.RecipeId = recipeId;
                var createdModel = await this.Repository.AddAsync(dbModel);
                result = Mapper.Map<StepViewModel>(createdModel);
                Logger.LogDebug($"New Step '{result.Id}' for recipe '{recipeId}' successfully created");
            }
            catch (Exception e)
            {
                var message = $"An Error occured while creating a new recipe step";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message);
            }

            return result;
        }

        /// <summary>
        /// Deletes an entity from the Database
        /// </summary>
        /// <param name="id">entity id</param>
        /// <returns></returns>
        public async Task RemoveAsync(int id)
        {
            try
            {
                var element = await this.Repository.FindAsync(id);
                if (element == null)
                {
                    throw new DataObjectNotFoundException();
                }
                await this.Repository.RemoveAsync(element);
                Logger.LogDebug($"recipe step '{id}' successfully deleted");
            }
            catch (Exception e)
            {
                var message = $"An Error occured while deleting the recipe step '{id}'";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message);
            }
        }

        /// <summary>
        /// Releases an returns unnessessary system resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this.Repository.Dispose();
                this._recipeRep.Dispose();
            }

            // release any unmanaged objects
            // set the object references to null
            Logger = null;
            Mapper = null;
            Repository = null;
            this._recipeRep = null;

            _disposed = true;
        }

        public async Task UpdateAsync(int id, StepUpdateViewModel model)
        {
            try
            {
                if (id.Equals(default(int))) { throw new ArgumentNullException(nameof(id)); }
                if (model == null) { throw new ArgumentNullException(nameof(model)); }
                if (!id.Equals(model.Id)) {  throw new IdsNotIdenticalException(); }

                var existingElement = await this.Repository.FindAsync(id);
                var newObject = Mapper.Map(model, existingElement);
                var updateResult = await Repository.UpdateAsync(newObject);
                Logger.LogDebug(new EventId(), updateResult ? $"recipe step {id} successfully updated" : $"No need to update step {id}");
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogWarning(new EventId(), e, $"No recipe step with id {id} found");
                throw new DataObjectNotFoundException();
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while updating a recipe step");
                throw new Exception($"An Error occured while updating a recipe step");
            }
        }

        public async Task UpdateAllStepsAsync(int recipeId, ICollection<string> model)
        {
            if (recipeId.Equals(default(int))) { throw new ArgumentNullException(nameof(recipeId)); }
            if (model == null) { throw new ArgumentNullException(nameof(model)); }
            await this.Repository.UpdateAllAsync(recipeId, model);
        }

        #endregion

        #region PROPERTIES

        protected IStepRepository Repository { get; set; }
        public IMapper Mapper { get; set; }
        public ILogger<RecipeStepService> Logger { get; set; }
        #endregion

    }
}