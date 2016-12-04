using System;
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
            RecipeConfiguration();
            IngrediantConfiguration();
        }

        #endregion

        #region METHODS
        private void RecipeImageConfiguration()
        {
            this.CreateMap<RecipeImage, RecipeImageViewModel>();
        }

        private void RecipeStepConfiguration()
        {
            this.CreateMap<StepCreateViewModel, RecipeStep>()
                .ForMember(x => x.Id, x => x.UseValue(0))
                .ForMember(x => x.Recipe, x => x.Ignore());

            this.CreateMap<RecipeStep, StepViewModel>()
                .ForMember(x => x.Url, x => x.Ignore());
            this.CreateMap<StepViewModel, RecipeStep>()
                .ForMember(x => x.Recipe, x => x.Ignore());
        }

        private void RecipeTagConfiguration()
        {
            this.CreateMap<string, RecipeTag>()
                .ForMember(x => x.Name, x => x.MapFrom(y => y))
                .ForMember(x => x.Id, x => x.UseValue(0))
                .ForMember(x => x.Recipes, x => x.Ignore());

            this.CreateMap<RecipeTag, NamedViewModel>()
                .ForMember(x => x.Url, x => x.Ignore());
            this.CreateMap<NamedViewModel, RecipeTag>()
                .ForMember(x => x.Recipes, x => x.Ignore());
        }

        private void RecipeSourceConfiguration()
        {
            this.CreateMap<WebSourceCreationViewModel, RecipeUrlSource>()
                .ForMember(x => x.Url, x => x.MapFrom(s => s.SourceUrl))
                .ForMember(x => x.Id, x => x.UseValue(0))
                .ForMember(x => x.RecipeSourceRecipes, x => x.Ignore());

            this.CreateMap<RecipeUrlSource, WebSourceViewModel>()
                .ForMember(x => x.Url, x => x.Ignore())
                .ForMember(x => x.SourceUrl, x => x.MapFrom(s => s.Url));
            ;
            this.CreateMap<WebSourceViewModel, RecipeUrlSource>()
                .ForMember(x => x.Url, x => x.MapFrom(s => s.SourceUrl))
                .ForMember(x => x.RecipeSourceRecipes, x => x.Ignore());

            this.CreateMap<CookbookSourceCreationViewModel, RecipeCookbookSource>()
                .ForMember(x => x.Id, x => x.UseValue(0))
                .ForMember(x => x.RecipeSourceRecipes, x => x.Ignore());
            ;
            this.CreateMap<CookbookSourceViewModel, RecipeCookbookSource>()
                .ForMember(x => x.RecipeSourceRecipes, x => x.Ignore());

            this.CreateMap<RecipeSourceRecipe, RecipeShortInfoViewModel>()
                .ForMember(x => x.Page, x => x.MapFrom(y => y.Page))
                .ForMember(x => x.Type, x => x.MapFrom(y => y.Source.GetSourceType()))
                .ForMember(x => x.Id, x => x.MapFrom(y => y.RecipeId))
                .ForMember(x => x.Name, x => x.MapFrom(y => y.Recipe.Name))
                .ForMember(x => x.Url, x => x.Ignore());

            this.CreateMap<RecipeCookbookSource, CookbookSourceViewModel>()
                .ConvertUsing<CookbookSourceConverter>();

            this.CreateMap<RecipeBaseSource, SourceListEntryViewModel>()
                .ForMember(x=>x.Url, x=>x.Ignore())
                .ForMember(x=>x.Type, x=>x.MapFrom(m => m.GetSourceType()))
                .ForMember(x=>x.CountRecipes, x=>x.MapFrom(m => m.RecipeSourceRecipes != null ? m.RecipeSourceRecipes.Count : 0));
    }

        private void RecipeConfiguration()
        {
            this.CreateMap<RecipeCreationViewModel, Recipe>()
                .ForMember(x => x.Id, x => x.UseValue(0))
                .ForMember(x => x.CookedCounter, x => x.UseValue(0))
                .ForMember(x => x.LastTimeCooked, x => x.UseValue(new DateTime()))
                .ForMember(x => x.SourceId, x => x.UseValue(0))
                .ForMember(x => x.Tags, x => x.Ignore())
                .ForMember(x => x.Source, x => x.Ignore())
                .ForMember(x => x.Ingrediants, x => x.Ignore())
                .ForMember(x => x.Steps, x => x.Ignore())
                .ForMember(x => x.Images, x => x.Ignore());

            this.CreateMap<Recipe, RecipeUpdateViewModel>()
                .ForMember(x => x.Url, x => x.Ignore());

            this.CreateMap<RecipeUpdateViewModel, Recipe>()
                .ForMember(x => x.CookedCounter, x => x.Ignore())
                .ForMember(x => x.LastTimeCooked, x => x.Ignore())
                .ForMember(x => x.SourceId, x => x.Ignore())
                .ForMember(x => x.Tags, x => x.Ignore())
                .ForMember(x => x.Source, x => x.Ignore())
                .ForMember(x => x.Ingrediants, x => x.Ignore())
                .ForMember(x => x.Steps, x => x.Ignore())
                .ForMember(x => x.Images, x => x.Ignore());

            this.CreateMap<Recipe, ShortRecipeViewModel>()
                .ForMember(x => x.ImageUrl, x => x.MapFrom(y => y.Images.FirstOrDefault() != null ? y.Images.FirstOrDefault().Url : null))
                .ForMember(x => x.Tags, x => x.MapFrom(y => y.Tags != null ? y.Tags.Select(tag => tag.RecipeTag.Name) : null))
                .ForMember(x => x.Url, x => x.Ignore());

            this.CreateMap<Recipe, FullRecipeViewModel>()
                .ForMember(x => x.Tags, y => y.MapFrom(x => x.Tags.Select(tag => new NamedViewModel() {Id = tag.RecipeTagId, Name = tag.RecipeTag.Name})))
                .ForMember(x => x.Images, y => y.MapFrom(x => x.Images.Select(image => new NamedViewModel() {Id = image.Id, Name = image.Filename, Url = image.Url})))
                .ForMember(x => x.Steps, y => y.MapFrom(x => x.Steps.Select(step => new StepViewModel()
                {
                    RecipeId = step.RecipeId,
                    Id = step.Id,
                    Description = step.Description,
                    Order = step.Order
                })))
                .ForMember(x => x.Ingrediants, y => y.MapFrom(x => x.Ingrediants.Select(ingrediant => new RecipeIngrediantViewModel()
                {
                    Name = ingrediant.Ingrediant.Name,
                    Id = ingrediant.IngrediantId,
                    Amount = ingrediant.Amount,
                    Unit = ingrediant.CookingUnit
                })))
                .ForMember(x => x.Type, y => y.MapFrom(x => new RecipeSourceShortInfoViewModel()
                {
                    Id = x.Source.SourceId,
                    Name = x.Source.Source.Name,
                    Page = x.Source.Page,
                    Type = x.Source.Source.GetSourceType()
                }))
                .ForMember(x => x.Url, x => x.Ignore());
        }

        private void IngrediantConfiguration()
        {
            this.CreateMap<Ingrediant, IngrediantStatisticViewModel>()
                .ForMember(x=>x.NumberOfRecipes, y=>y.MapFrom(x=> x.RecipeIngrediants.Count))
                .ForMember(x=>x.Url, x=>x.Ignore());

            this.CreateMap<RecipeIngrediant, IngrediantViewModel>()
                .ForMember(x => x.Id, y => y.MapFrom(x => x.IngrediantId))
                .ForMember(x => x.Name, y => y.MapFrom(x => x.Ingrediant.Name))
                .ForMember(x => x.Unit, y => y.MapFrom(x => x.CookingUnit))
                .ForMember(x => x.Url, y => y.Ignore());

            this.CreateMap<string, NamedViewModel>()
                .ForMember(x => x.Name, y => y.MapFrom(x => x))
                .ForMember(x => x.Id, y => y.UseValue(0))
                .ForMember(x => x.Url, y => y.Ignore());
            
            this.CreateMap<NamedViewModel, Ingrediant>()
                .ForMember(x=>x.RecipeIngrediants, x=>x.Ignore());

            this.CreateMap<Ingrediant, NamedViewModel>()
                .ForMember(x=>x.Url, x=>x.Ignore());
        }


        #endregion

        #region PROPERTIES
        #endregion
    }
}
