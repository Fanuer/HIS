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
    internal class TagService : BaseService<ITagsRepository, RecipeTag, NamedViewModel, string>, ITagService
    {
        #region CONST

        #endregion

        #region FIELDS

        private IRecipeRepository _recipeRep;

        #endregion

        #region CTOR

        public TagService(ITagsRepository rep, IRecipeRepository recipeRep, IMapper mapper, ILoggerFactory loggerFactory)
            : base(rep, mapper, loggerFactory?.CreateLogger<TagService>(), "recipe tag")
        {
            _recipeRep = recipeRep;
        }

        #endregion

        #region METHODS

        public IQueryable<NamedViewModel> GetTagsAsync()
        {
            IQueryable<NamedViewModel> result;
            try
            {
                result = this.Repository.GetAll().ProjectTo<NamedViewModel>(this.Mapper.ConfigurationProvider);
                this.Logger.LogInformation("Recived all tags successfully");
            }
            catch (Exception e)
            {
                var message = "Error on receiving all tags";
                Logger.LogError(new EventId(), e, message);
                throw e;
            }

            return result;
        }

        public override async Task<NamedViewModel> AddAsync(string creationModel)
        {
            NamedViewModel result;

            try
            {
                var existing = await GetTagByNameAsync(creationModel);
                result = existing ?? await base.AddAsync(creationModel);
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }

        public async Task AddTagToRecipeAsync(int recipeId, string tagName)
        {
            try
            {
                var tag = await this.AddAsync(tagName);
                var dbTag = await this.Repository.FindAsync(tag.Id, x=>x.Recipes);

                var recipe = await _recipeRep.FindAsync(recipeId);
                if (recipe == null)
                {
                    throw new DataObjectNotFoundException($"No Recipe with id {recipeId} found to add Tag '{tagName}'");
                }

                dbTag.Recipes.Add(new RecipeRecipeTag(recipeId, dbTag.Id));
                await this.Repository.SaveChangesAsync();
                Logger.LogInformation(new EventId(), $"Tag '{tagName}' added to recipe '{recipe.Name}({recipeId})'");
            }
            catch (Exception e)
            {
                var message = $"An Error occurs while adding tag '{tagName}' from recipe {recipeId}";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
        }

        public async Task RemoveTagToRecipeAsync(int recipeId, string tagName)
        {
            try
            {
                var dbTag = await Repository.GetAll().Include(x => x.Recipes).SingleOrDefaultAsync(x => x.Name.Equals(tagName));

                var recipeTag = dbTag?.Recipes.FirstOrDefault(x => x.RecipeId.Equals(recipeId) && x.RecipeTagId.Equals(dbTag.Id));
                if (recipeTag != null)
                {
                    dbTag.Recipes.Remove(recipeTag);
                    await this.Repository.SaveChangesAsync();
                    this.Logger.LogInformation($"Removed tag '{tagName}' from recipe '{recipeTag.Recipe.Name}({recipeTag.RecipeId})'");
                }
            }
            catch (Exception e)
            {
                var message = $"An Error occurs while removing tag '{tagName}' from recipe {recipeId}";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
            
        }

        private async Task<NamedViewModel> GetTagByNameAsync(string tagName)
        {
            if (String.IsNullOrWhiteSpace(tagName)) { throw new ArgumentNullException(nameof(tagName)); }
            var result = await this.Repository.GetAll().FirstOrDefaultAsync(x => x.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase));
            return result != null ? Mapper.Map<NamedViewModel>(result) : null;
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
