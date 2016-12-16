using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Helpers.Exceptions;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using AutoMapper.QueryableExtensions;

namespace HIS.Recipes.Services.Implementation.Services
{
    internal class RecipeImagesService: IRecipeImageService
    {
        #region CONST

        #endregion

        #region FIELDS

        private bool _disposed;

        #endregion

        #region CTOR

        internal RecipeImagesService(IDbImageRepository rep, IImageService imageService, IMapper mapper, ILoggerFactory loggerFactory)
        {
            if (rep == null)
            {
                throw new ArgumentNullException(nameof(rep));
            }
            if (mapper == null)
            {
                throw new ArgumentNullException(nameof(mapper));
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            if (imageService == null)
            {
                throw new ArgumentNullException(nameof(imageService));
            }
            Repository = rep;
            Mapper = mapper;
            Logger = loggerFactory.CreateLogger<RecipeImagesService>();
            this.ImageService = imageService;
        }

        ~RecipeImagesService()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS

        /// <summary>
        /// Updates a database entity with the given data from a view model
        /// </summary>
        /// <param name="id">Database id</param>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="data">New image data</param>
        /// <returns></returns>
        public async Task UpdateAsync(int id, int recipeId, IFormFile data)
        {
            try
            {
                if (id.Equals(0)) { throw new ArgumentNullException(nameof(id)); }
                if (data == null) { throw new ArgumentNullException(nameof(data)); }
                
                var existingElement = await this.Repository.FindAsync(id);
                
                // image must be from given recipe
                if (!existingElement.RecipeId.Equals(recipeId))
                {
                    throw new DataObjectNotFoundException();
                }
                if (!existingElement.Filename.Equals(data.FileName))
                {
                    var oldFileName = existingElement.Filename;
                    await this.ImageService.RemoveImageAsync(existingElement.RecipeId, existingElement.Filename);
                    existingElement.Filename = data.FileName;
                    existingElement.Url = await this.ImageService.UploadImageAsync(existingElement.RecipeId, data);
                    await Repository.UpdateAsync(existingElement);
                    Logger.LogInformation($"Recipe {recipeId}: Image updated from '{oldFileName}' to '{existingElement?.Filename}'");
                }
                else
                {
                    existingElement.Url = await this.ImageService.UploadImageAsync(existingElement.RecipeId, data);
                    var updateResult = await Repository.UpdateAsync(existingElement);
                    Logger.LogDebug(new EventId(), updateResult ? $"recipe image {id} successfully updated" : $"No need to update step {id}");
                }
            }
            catch (DbUpdateConcurrencyException e)
            {
                Logger.LogWarning(new EventId(), e, $"No recipe image with id {id} found");
                throw new DataObjectNotFoundException($"No recipe image with id {id} found");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while updating a recipe image");
                throw new Exception($"An Error occured while updating a recipe image");
            }
        }

        /// <summary>
        /// Creates a new entity in the Database
        /// </summary>
        /// <param name="recipeId">Id of the owning recipe</param>
        /// <param name="creationModel">entity data</param>
        /// <returns></returns>
        public async Task<RecipeImageViewModel> AddAsync(int recipeId, IFormFile creationModel)
        {
            RecipeImageViewModel result;

            try
            {
                var url = await this.ImageService.UploadImageAsync(recipeId, creationModel);
                var datamodel = new RecipeImage()
                {
                    RecipeId = recipeId,
                    Filename = creationModel.FileName,
                    Url = url
                };
                await Repository.AddAsync(datamodel);
                result = this.Mapper.Map<RecipeImageViewModel>(datamodel);
                Logger.LogDebug($"New recipe image '{datamodel.Id}' successfully created");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while creating a recipe image");
                throw new Exception($"An Error occured while creating a recipe image");
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
                if (element == null) { throw new DataObjectNotFoundException(); }
                await this.ImageService.RemoveImageAsync(element.RecipeId, element.Filename);
                await this.Repository.RemoveAsync(element);
                Logger.LogDebug($"recipe image '{id}' successfully deleted");
            }
            catch (Exception e)
            {
                Logger.LogError(new EventId(), e, $"An Error occured while deleting the recipe image '{id}'");
                throw new Exception($"An Error occured while deleting the recipe image '{id}'");
            }
        }

        /// <summary>
        /// Get all recipe images
        /// </summary>
        /// <param name="recipeId">id if the recipe</param>
        /// <returns></returns>
        public IQueryable<RecipeImageViewModel> GetImages(int recipeId)
        {
            IQueryable<RecipeImageViewModel> result = null;
            try
            {
                result = this.Repository
                            .GetAll()
                            .Where(x => x.RecipeId.Equals(recipeId))
                            .ProjectTo<RecipeImageViewModel>(this.Mapper.ConfigurationProvider);

                this.Logger.LogDebug(new EventId(), $"Returned all images of recipe {recipeId}");
            }
            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving images for recipe {recipeId}");
                throw new Exception($"Error on receiving images for recipe {recipeId}");
            }
            return result;
        }

        /// <summary>
        /// Returns an image 
        /// </summary>
        /// <param name="imageId"></param>
        /// <returns></returns>
        public async Task<RecipeImageViewModel> GetImage(int imageId)
        {
            RecipeImageViewModel result = null;
            try
            {
                var dataobject = await this.Repository.FindAsync(imageId);
                if (dataobject == null)
                {
                    throw new DataObjectNotFoundException($"No image with the given id '{imageId}' found");
                }
                result = Mapper.Map<RecipeImageViewModel>(dataobject);
                this.Logger.LogDebug(new EventId(), $"Returned iamge '{imageId}' of recipe {dataobject.RecipeId}");
            }

            catch (Exception e)
            {
                this.Logger.LogError(new EventId(), e, $"Error on receiving image '{imageId}'");
                throw new Exception($"Error on receiving image '{imageId}'");
            }
            return result;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                this.ImageService.Dispose();
                this.Repository.Dispose();
            }

            this.ImageService = null;
            this.Repository = null;
            this.Logger = null;
            this.Mapper = null;

            _disposed = true;
        }

        /// <summary>
        /// Releases an returns unnessessary system resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region PROPERTIES
        protected IMapper Mapper { get; private set; }
        protected ILogger Logger { get; private set; }
        protected IImageService ImageService { get; private set; }
        protected virtual IDbImageRepository Repository { get; private set; }
        #endregion  
    }
}
