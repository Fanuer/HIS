using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Recipes.Services.Interfaces.Services;
using Microsoft.AspNetCore.Http;

namespace HIS.Recipes.Services.Tests.Helper
{
    public class MockImageService:IImageService
    {
        public const string URIFORMAT= "http://www.mytestservice.de/image/{0}/{1}";

        public void Dispose()
        {
        }

        public Task<string> UploadImageAsync(int recipeId, IFormFile file)
        {
            return Task.FromResult(String.Format(URIFORMAT, recipeId, file.FileName));
        }

        public Task RemoveImageAsync(int recipeId, string imageFilename)
        {
            return Task.FromResult(0);
        }
    }
}
