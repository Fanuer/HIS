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

        public IQueryable<NamedViewModel> GetTags()
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
            var tag = await this.AddAsync(tagName);
            await AddTagToRecipeAsync(recipeId, tag.Id);
        }

        public async Task AddTagToRecipeAsync(int recipeId, int tagId)
        {
            var dbTag = await this.Repository.FindAsync(tagId, x => x.Recipes);
            await this.AddTagToRecipeAsync(recipeId, dbTag);
        }

        public async Task RemoveTagFromRecipeAsync(int recipeId, int tagId)
        {
            var tag = await this.Repository.FindAsync(tagId, x=>x.Recipes);
            await RemoveTagFromRecipeAsync(recipeId, tag);
        }

        public async Task RemoveTagFromRecipeAsync(int recipeId, string tagName)
        {
            if (String.IsNullOrEmpty(tagName)){throw new ArgumentNullException(nameof(tagName));}
            var tag = await this.Repository.GetAll().Include(x=>x.Recipes).FirstOrDefaultAsync(x => x.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase));
            await RemoveTagFromRecipeAsync(recipeId, tag);
        }

        private async Task AddTagToRecipeAsync(int recipeId, RecipeTag tag)
        {
            try
            {
                if (tag == null) { throw new ArgumentNullException(nameof(tag)); }
                var recipe = await _recipeRep.FindAsync(recipeId, x => x.Tags);
                if (recipe == null)
                {
                    throw new DataObjectNotFoundException($"No Recipe with id {recipeId} found to add Tag '{tag.Name}'");
                }
                if (recipe.Tags.All(x => x.RecipeTagId != tag.Id))
                {
                    tag.Recipes.Add(new RecipeRecipeTag(recipeId, tag.Id));
                    await this.Repository.SaveChangesAsync();
                    Logger.LogDebug(new EventId(), $"Tag '{tag.Name}' added to recipe '{recipe.Name}({recipeId})'");
                }
                else
                {
                    Logger.LogWarning(new EventId(), $"Tag '{tag.Name}' already available in recipe '{recipe.Name}({recipeId})'");
                }
            }
            catch (Exception e)
            {
                var message = $"An Error occurs while adding tag '{tag?.Name}' from recipe {recipeId}";
                Logger.LogError(new EventId(), e, message);
                throw new Exception(message, e);
            }
        }

        private async Task RemoveTagFromRecipeAsync(int recipeId, RecipeTag tag)
        {
            try
            {
                if (tag == null) { throw new ArgumentNullException(nameof(tag));}
                
                var recipeTag = tag?.Recipes.FirstOrDefault(x => x.RecipeId.Equals(recipeId) && x.RecipeTagId.Equals(tag.Id));
                if (recipeTag != null)
                {
                    tag.Recipes.Remove(recipeTag);
                    await this.Repository.SaveChangesAsync();
                    this.Logger.LogDebug($"Removed tag '{tag.Name}' from recipe '{recipeTag.RecipeId}'");
                }
                else
                {
                    this.Logger.LogWarning($"Tried removing tag '{tag.Name}' from recipe '{recipeId}', but was not found");
                }
            }
            catch (Exception e)
            {
                var message = $"An Error occurs while removing tag '{tag?.Name}' from recipe {recipeId}";
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
