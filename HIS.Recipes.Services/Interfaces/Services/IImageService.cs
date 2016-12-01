using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace HIS.Recipes.Services.Interfaces.Services
{
    /// <summary>
    /// Creates access to a image storage for recipes
    /// </summary>
    public interface IImageService: IDisposable
    {
        /// <summary>
        /// Uploads an Image to the image store
        /// </summary>
        /// <param name="recipeId">owning recpie Id</param>
        /// <param name="file">image data</param>
        /// <returns></returns>
        Task<string> UploadImageAsync(Guid recipeId, IFormFile file);
        /// <summary>
        /// Removes an image from the image store
        /// </summary>
        /// <param name="recipeId">owning recpie Id</param>
        /// <param name="imageFilename">image name</param>
        /// <returns></returns>
        Task RemoveImageAsync(Guid recipeId, string imageFilename);
    }
}
