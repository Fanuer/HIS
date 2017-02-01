using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AutoMapper;
using HIS.Helpers.Test;
using HIS.Recipes.Services.Configs;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Implementation.Services;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Tests.Helper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace HIS.Recipes.Services.Tests.ServiceTests
{
    public class RecipeImageServiceTest:TestBase
    {
        [Fact]
        public async Task GetImages()
        {
            await this.InitializeAsync();
            using (var service = this.GetService())
            {
                var recipe = await this.DbContext.Recipes.Include(x => x.Images).FirstAsync(x => x.Images.Any());
                var images = await service.GetImages(recipe.Id).ToListAsync();
                
                Assert.NotNull(images);
                Assert.NotEmpty(images);

                var firstImage = images.First();
                var firstDbImage = recipe.Images.First();

                Assert.Equal(firstDbImage.Id, firstImage.Id);
                Assert.Equal(firstDbImage.Filename, firstImage.Filename);
                Assert.Equal(firstDbImage.Url, firstImage.Url);
            }
        }

        [Fact]
        public async Task GetImage()
        {
            await this.InitializeAsync();
            using (var service = this.GetService())
            {
                var dbImage = await this.DbContext.RecipeImages.FirstAsync();
                var image = await service.GetImage(dbImage.Id);

                Assert.NotNull(image);

                Assert.Equal(dbImage.Id, image.Id);
                Assert.Equal(dbImage.Filename, image.Filename);
                Assert.Equal(dbImage.Url, image.Url);
            }
        }

        [Fact]
        public async Task AddImage()
        {
            await this.InitializeAsync();
            using (var service = this.GetService())
            {
                var fileMock = new Mock<IFormFile>();
                //Setup mock file using a memory stream
                var s = "Hello World from a Fake File";
                var ms = new MemoryStream();

                var writer = new StreamWriter(ms);
                writer.Write(s);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
                fileMock.SetupGet(x => x.FileName).Returns("Test.jpg");

                var recipe = await this.DbContext.Recipes.FirstAsync();
                var image = await service.AddAsync(recipe.Id, fileMock.Object);
                
                Assert.NotNull(image);
                Assert.Equal(this.DbContext.RecipeImages.Count(), this.TestData.Images.Count +1);
                var dbImage = await this.DbContext.RecipeImages.FirstOrDefaultAsync(x => x.Id.Equals(image.Id));

                Assert.Equal(dbImage.Id, image.Id);
                Assert.Equal(dbImage.Filename, image.Filename);
                Assert.Equal(dbImage.Url, image.Url);
                Assert.Equal(dbImage.RecipeId, image.RecipeId);
            }
        }

        [Fact]
        public async Task UpdateImage()
        {
            await this.InitializeAsync();
            using (var service = this.GetService())
            {
                var availableImage = await DbContext.RecipeImages.AsNoTracking().FirstAsync();
                string fileName = "Test.jpg";
                var fileMock = new Mock<IFormFile>();
                //Setup mock file using a memory stream
                var s = "Hello World from a Fake File";
                var ms = new MemoryStream();

                var writer = new StreamWriter(ms);
                writer.Write(s);
                writer.Flush();
                ms.Position = 0;
                fileMock.Setup(m => m.OpenReadStream()).Returns(ms);
                fileMock.SetupGet(x => x.FileName).Returns(fileName);
                
                await service.UpdateAsync(availableImage.Id, availableImage.RecipeId, fileMock.Object);
                var updatedImage = await DbContext.RecipeImages.AsNoTracking().FirstAsync();

                Assert.NotNull(updatedImage);
                
                Assert.Equal(availableImage.Id, updatedImage.Id);
                Assert.Equal(availableImage.RecipeId, updatedImage.RecipeId);
                Assert.NotEqual(availableImage.Filename, updatedImage.Filename);
                Assert.Equal(fileName, updatedImage.Filename);
                Assert.NotEqual(availableImage.Url, updatedImage.Url);
            }
        }

        [Fact]
        public async Task RemoveImage()
        {
            await this.InitializeAsync();
            using (var service = this.GetService())
            {
                var availableImage = await DbContext.RecipeImages.AsNoTracking().FirstAsync();

                await service.RemoveAsync(availableImage.Id);
                var updatedImage = await DbContext.RecipeImages.AsNoTracking().FirstOrDefaultAsync(x=>x.Id.Equals(availableImage.Id));

                Assert.Null(updatedImage);
                Assert.Equal(DbContext.RecipeImages.Count(), this.TestData.Images.Count -1);
            }
        }
        

        private IRecipeImageService GetService()
        {
            var rep = new RecipeDbRepository.DbImageRepository(this.DbContext, new MockLoggerFactory<object>());
            var mockFactory = new MockLoggerFactory<IngrediantService>();
            IMapper mapper = new Mapper(new MapperConfiguration(m => m.AddProfile<AutoMapperServiceProfile>()));
            IImageService mockImageService = new MockImageService();
            return new RecipeImagesService(rep, mockImageService, mapper, mockFactory);
        }
    }
}
