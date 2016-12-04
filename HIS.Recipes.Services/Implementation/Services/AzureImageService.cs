using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace HIS.Recipes.Services.Implementation.Services
{
    /// <summary>
    /// Grants Access to an image store for recipes by using an azure blob storage
    /// </summary>
    public class AzureImageService : IImageService
    {
        #region CONST

        private const string RecipeContainerPrefix = "recipe_";

        #endregion

        #region FIELDS

        private CloudBlobContainer _baseContainer;
        private bool _intialized = false;
        private bool _disposed;
        private readonly ILogger<AzureImageService> _log;

        #endregion

        #region CTOR
        /// <summary>
        /// Creates an Image Services object
        /// </summary>
        /// <param name="options">options to grant access to the azure blob storage</param>
        /// <param name="loggerFactory">logging factory to create a logger with</param>
        public AzureImageService(IOptions<AzureBlobStorageOptions> options, ILoggerFactory loggerFactory)
        {
            if (options == null) { throw new ArgumentNullException(nameof(options)); }
            if (options.Value == null) { throw new ArgumentNullException(nameof(options), "No Options for Azure Bob Storage defined"); }
            if (String.IsNullOrEmpty(options.Value?.BlobStorageConnectionString)) { throw new ArgumentNullException(nameof(options.Value.BlobStorageConnectionString), "Connectionstring for Azure Blob Storage must be defined");}

            var storageAccount = CloudStorageAccount.Parse(options.Value.BlobStorageConnectionString);
            var client = storageAccount.CreateCloudBlobClient();
            var baseContainerName = (options.Value.BaseContainer ?? "recipeImages").ToLower();
            _baseContainer = client.GetContainerReference(baseContainerName);

            _log = loggerFactory.CreateLogger<AzureImageService>();
        }
        
        ~AzureImageService()
        {
            Dispose(false);
        }

        #endregion

        #region METHODS
        /// <summary>
        /// Uploads an Image to the image store
        /// </summary>
        /// <param name="recipeId">owning recpie Id</param>
        /// <param name="file">image data</param>
        /// <returns></returns>
        public async Task<string> UploadImageAsync(int recipeId, IFormFile file)
        {
            CloudBlockBlob blockBlob = null;
            try
            {
                if (file == null)
                {
                    throw new ArgumentNullException(nameof(file));
                }
                if (String.IsNullOrWhiteSpace(file.FileName))
                {
                    throw new ArgumentNullException(nameof(file), "Filename must not be an empty string");
                }

                if (!_intialized)
                {
                    await InitAsync();
                }

                var blobName = GetBlobName(recipeId, file.FileName);
                blockBlob = _baseContainer.GetBlockBlobReference(blobName);

                using (var fileStream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }
                _log.LogDebug($"Recipe {recipeId}: Image '{file.FileName}' successfully updated");
            }
            catch (Exception e)
            {
                _log.LogError(new EventId(), e, $"Recipe {recipeId}: Error on uploading image: {e.Message}");
                throw new Exception("Error on uploading image", e);
            }

            return blockBlob?.StorageUri.ToString();
        }

        /// <summary>
        /// Removes an image from the image store
        /// </summary>
        /// <param name="recipeId">owning recpie Id</param>
        /// <param name="imageFilename">image name</param>
        /// <returns></returns>
        public async Task RemoveImageAsync(int recipeId, string imageFilename)
        {
            try
            {
                if (!_intialized) { await InitAsync(); }

                var blobName = GetBlobName(recipeId, imageFilename);
                var file = await _baseContainer.GetBlobReferenceFromServerAsync(blobName);
                var result = await file.DeleteIfExistsAsync();
                _log.LogDebug(result
                    ? $"Recipe {recipeId}: Image '{imageFilename}' successfully deleted"
                    : $"Recipe {recipeId}: No Deletion: No image '{imageFilename}' found");
            }
            catch (Exception e)
            {
                _log.LogError(new EventId(), e, $"Recipe {recipeId}: Error on uploading image: {e.Message}");
                throw new Exception("Error on deleting an image"); ;
            }
        }
        
        /// <summary>
        /// Initialises the base container
        /// </summary>
        /// <returns></returns>
        private async Task InitAsync()
        {
            // Create the container if it doesn't already exist.
            await _baseContainer.CreateIfNotExistsAsync();
            await _baseContainer.SetPermissionsAsync(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });

            _intialized = true;
        }

        private string GetBlobName(int recipeId, string imageFilename)
        {
            if (String.IsNullOrWhiteSpace(imageFilename)) { throw new ArgumentNullException(nameof(imageFilename)); }
            if (recipeId.Equals(0)) { throw new ArgumentNullException(nameof(recipeId)); }

            return String.Concat(RecipeContainerPrefix, recipeId.ToString(), "_", imageFilename);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) { 
                return;
            }

            this._baseContainer = null;

            _disposed = true;
        }

        /// <summary>
        /// Disposes all resources
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
