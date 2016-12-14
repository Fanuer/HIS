using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
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
    internal class IngrediantService : BaseService<IIngrediantRepository, Ingrediant, NamedViewModel, string>, IIngrediantService
    {
        #region CONST

        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public IngrediantService(IIngrediantRepository rep, IMapper mapper, ILoggerFactory loggerfactory)
            : base(rep, mapper, loggerfactory.CreateLogger<IngrediantService>(), "ingrediant")
        {
        }

        #endregion

        #region METHODS

        public IQueryable<IngrediantStatisticViewModel> GetIngrediantList()
        {
            IQueryable<IngrediantStatisticViewModel> result = null;
            try
            {
                result = this.Repository
                    .GetAll()
                    .ProjectTo<IngrediantStatisticViewModel>(Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all ingrediants");
            }
            catch (Exception e)
            {
                var message = $"Error on receiving the ingrediant list";
                this.Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
            return result;
        }

        public IQueryable<IngrediantViewModel> GetIngrediantsForRecipe(int recipeId)
        {
            IQueryable<IngrediantViewModel> result = null;
            try
            {
                result = this.Repository
                    .GetAll()
                    .Include(x => x.Recipes)
                    .SelectMany(x => x.Recipes)
                    .Where(x => x.RecipeId.Equals(recipeId))
                    .Include(x => x.Ingrediant)
                    .ProjectTo<IngrediantViewModel>(this.Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all ingrediants for recipe '{recipeId}'");
            }
            catch (Exception e)
            {
                var message = $"Error on receiving the ingrediant list for recipe '{recipeId}'";
                this.Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
            return result;
        }


        public async Task AddOrUpdateIngrediantToRecipeAsync(AlterIngrediantViewModel model)
        {
            try
            {
                if (model == null){ throw new ArgumentNullException(nameof(model)); }

                var ingrediant = await Repository.FindAsync(model.IngrediantId, x => x.Recipes);
                if (ingrediant== null)
                {
                    throw new DataObjectNotFoundException($"No Ingrediant with the id {model.IngrediantId} found");
                }
                var recipeIngrediant = ingrediant.Recipes.FirstOrDefault(x => x.RecipeId.Equals(model.RecipeId));
                if (recipeIngrediant == null)
                {
                    ingrediant.Recipes.Add(new RecipeIngrediant()
                    {
                        RecipeId = model.RecipeId,
                        Amount = model.Amount,
                        CookingUnit = model.Unit,
                        IngrediantId = ingrediant.Id
                    });
                }
                else
                {
                    recipeIngrediant.CookingUnit = model.Unit;
                    recipeIngrediant.Amount = model.Amount;
                }
                await Repository.SaveChangesAsync();
                Logger.LogDebug($"Recipe '{model.RecipeId}': Changed Ingrediant '{ingrediant.Name}({ingrediant.Id}) successfully'");
            }
            catch (Exception e)
            {
                var message = String.Concat("Error occured on altering ", (model?.IngrediantId.ToString() ?? "an ingrediant"), " for recipe ", (model?.RecipeId.ToString() ?? "a recipe"));
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
            
        }

        public async Task RemoveIngrediantFromRecipeAsync(int recipeId, int ingrediantId)
        {
            try
            {
                var ingrediant = await Repository.FindAsync(ingrediantId, x => x.Recipes);
                var recipeIngrediant = ingrediant.Recipes.FirstOrDefault(x => x.RecipeId.Equals(recipeId));
                if (recipeIngrediant != null)
                {
                    ingrediant.Recipes.Remove(recipeIngrediant);
                    await Repository.SaveChangesAsync();
                    Logger.LogDebug($"Removed '{ingrediant.Name}({ingrediantId})' from recipe '{recipeId}' successfully");
                }
                else
                {
                    Logger.LogInformation($"Tried removing '{ingrediant.Name}({ingrediantId})' from recipe '{recipeId}' but was not related to this recipe");
                }
            }
            catch (Exception e)
            {
                var message = $"Error occured on removing ingrediant '{ingrediantId}' from recipe '{recipeId}'";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
        }
        #endregion

        #region PROPERTIES
        #endregion
    }
}
