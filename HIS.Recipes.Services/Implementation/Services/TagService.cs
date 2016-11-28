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
using HIS.Recipes.Services.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HIS.Recipes.Services.Implementation.Services
{
    internal class TagService : BaseService<ITagsRepository, RecipeTag, TagViewModel, string>
    {
        #region CONST

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        public TagService(ITagsRepository rep, IMapper mapper, ILogger logger)
            : base(rep, mapper, logger, "recipe tag")
        {
        }

        #endregion

        #region METHODS

        public IQueryable<TagViewModel> GetTagsAsync()
        {
            return this.Repository.GetAll().ProjectTo<TagViewModel>();
        }

        public override async Task<TagViewModel> AddAsync(string creationModel)
        {
            TagViewModel result;

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

        public async Task AddTagToRecipeAsync(Guid recipeId, string tagName)
        {
            var tag = await this.AddAsync(tagName);
            var dbTag = await Repository
                                .GetAll()
                                .Include(x => x.Recipes)
                                .SingleOrDefaultAsync(x => x.Name.Equals(tagName));

            var recipe = dbTag
                            .Recipes
                            .Where(x => x.RecipeId.Equals(recipeId))
                            .Select(x => x.Recipe)
                            .FirstOrDefault();

            if (recipe == null)
            {
                throw new DataObjectNotFoundException($"No recipe with id '{recipeId}' found to add Tag '{tagName}'");
            }
            recipe.Tags.Add(new RecipeRecipeTag(recipe, dbTag));
            await this.Repository.SaveChangesAsync();
        }

        public async Task RemoveTagToRecipeAsync(Guid recipeId, string tagName)
        {
            var dbTag = await Repository.GetAll().Include(x => x.Recipes).SingleOrDefaultAsync(x => x.Name.Equals(tagName));
            if (dbTag == null) { return; }

            var recipeTag = dbTag.Recipes.FirstOrDefault(x => x.RecipeId.Equals(recipeId) && x.RecipeTagId.Equals(dbTag.Id));
            if (recipeTag != null)
            {
                dbTag.Recipes.Remove(recipeTag);
                await this.Repository.SaveChangesAsync();
            }
        }

        private async Task<TagViewModel> GetTagByNameAsync(string tagName)
        {
            if (String.IsNullOrWhiteSpace(tagName)) { throw new ArgumentNullException(nameof(tagName)); }
            var result = await this.Repository.GetAll().FirstOrDefaultAsync(x => x.Name.Equals(tagName, StringComparison.CurrentCultureIgnoreCase));
            return result != null ? Mapper.Map<TagViewModel>(result) : null;
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
