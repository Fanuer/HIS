using System.Linq;
using AutoMapper;
using HIS.Recipes.Models.Enums;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Configs.Converter;
using HIS.Recipes.Services.DB;
using HIS.Recipes.Services.Implementation.Repositories;
using HIS.Recipes.Services.Interfaces.Repositories;
using HIS.Recipes.Services.Interfaces.Services;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Configs
{
    public class AutoMapperServiceProfile: Profile
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public AutoMapperServiceProfile()
        {
            // Fuer BaseService: CVM -> DM, DM <-> VM

            RecipeImageConfiguration();
            RecipeStepConfiguration();
            RecipeTagConfiguration();
            RecipeSourceConfiguration();
        }

        #endregion

        #region METHODS
        private void RecipeImageConfiguration()
        {
            this.CreateMap<RecipeImage, RecipeImageViewModel>();
        }

        private void RecipeStepConfiguration()
        {
            this.CreateMap<StepCreateViewModel, RecipeStep>();
            this.CreateMap<RecipeStep, StepViewModel>();
            this.CreateMap<StepViewModel, RecipeStep>();
        }

        private void RecipeTagConfiguration()
        {
            this.CreateMap<string, RecipeTag>().ConstructProjectionUsing(x => new RecipeTag() { Name = x });
            this.CreateMap<RecipeTag, NamedViewModel>();
            this.CreateMap<NamedViewModel, RecipeTag>();
        }

        private void RecipeSourceConfiguration()
        {
            this.CreateMap<WebSourceCreationViewModel, RecipeUrlSource>()
                .ConstructProjectionUsing(x => new RecipeUrlSource()
                {
                    Name = x.Name,
                    Url = x.SourceUrl
                });
            this.CreateMap<RecipeUrlSource, WebSourceViewModel>().ConstructUsing(x => new WebSourceViewModel()
            {
                Name = x.Name,
                Id = x.Id,
                SourceUrl = x.Url
            });
            ;
            this.CreateMap<WebSourceViewModel, RecipeUrlSource>().ConstructUsing(x => new RecipeUrlSource()
            {
                Name = x.Name,
                Id = x.Id,
                Url = x.SourceUrl
            });

            this.CreateMap<CookbookSourceCreationViewModel, RecipeCookbookSource>();
            this.CreateMap<CookbookSourceViewModel, RecipeCookbookSource>();
            this.CreateMap<RecipeSourceRecipe, RecipeSourceShortInfoViewModel>()
                .ConstructProjectionUsing(x => new RecipeSourceShortInfoViewModel()
                {
                    Id = x.RecipeId,
                    Name = x.Recipe.Name,
                    Page = x.Page
                });

            this.CreateMap<RecipeCookbookSource, CookbookSourceViewModel>().ConvertUsing<CookbookSourceConverter>();

            this.CreateMap<RecipeBaseSource, SourceListEntryViewModel>()
                .ConstructUsing(m => new SourceListEntryViewModel()
                    {
                        Id = m.Id,
                        Name = m.Name,
                        CountRecipes = m.RecipeSourceRecipes?.Count ?? 0,
                        Type = m is RecipeCookbookSource ? SourceType.Cookbook : SourceType.WebSource
                    });

            

    }

        private void RecipeConfiguration()
        {
            this.CreateMap<RecipeCreationViewModel, Recipe>();
            this.CreateMap<RecipeStep, RecipeUpdateViewModel>();
            this.CreateMap<RecipeUpdateViewModel, RecipeUpdateViewModel>();
            this.CreateMap<Recipe, ShortRecipeViewModel>()
                .ConstructUsing(x =>
            {
                return new ShortRecipeViewModel()
                {
                    Id = x.Id,
                    ImageUrl = x.Images.FirstOrDefault()?.Url,
                    LastTimeCooked = x.LastTimeCooked,
                    Name = x.Name,
                    Tags = x.Tags?.Select(tag => tag.RecipeTag.Name)
                };
            });
            this.CreateMap<Recipe, FullRecipeViewModel>().ConstructUsing(x =>
            {
                return new FullRecipeViewModel()
                {
                    Calories = x.Calories,
                    Id = x.Id,
                    Name = x.Name,
                    Tags = x.Tags.Select(tag => new NamedViewModel() {Id = tag.RecipeTagId, Name = tag.RecipeTag.Name}),
                    CookedCounter = x.CookedCounter,
                    Creator = x.Creator,
                    Images = x.Images.Select(image => new NamedViewModel() {Id = image.Id, Name = image.Filename, Url = image.Url}),
                    LastTimeCooked = x.LastTimeCooked,
                    NumberOfServings = x.NumberOfServings,
                    Steps = x.Steps.Select(step =>new StepViewModel()
                                {
                                    Id = step.Id,
                                    Description = step.Description,
                                    Order = step.Order
                                }),
                    Ingrediants = x.Ingrediants.Select( ingrediant => new RecipeIngrediantViewModel()
                                {
                                    Name = ingrediant.Ingrediant.Name,
                                    Id = ingrediant.IngrediantId,
                                    Amount = ingrediant.Amount,
                                    Unit = ingrediant.CookingUnit
                                }),
                    Type = new RecipeSourceShortInfoViewModel()
                    {
                      Id  = x.Source.SourceId,
                      Name = x.Source.Source.Name,
                      Page = x.Source.Page,
                      Type = x.Source.Source is RecipeCookbookSource ? SourceType.Cookbook : SourceType.WebSource
                    } 
                };
            });
        }

        #endregion

        #region PROPERTIES
        #endregion
    }
}
