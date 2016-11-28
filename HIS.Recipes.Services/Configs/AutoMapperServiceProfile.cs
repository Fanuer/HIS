using AutoMapper;
using HIS.Recipes.Models.ViewModels;
using HIS.Recipes.Services.Models;

namespace HIS.Recipes.Services.Configs
{
    public class AutoMapperServiceProfile: Profile
    {
        public AutoMapperServiceProfile()
        {
            this.CreateMap<RecipeImage, RecipeImageViewModel>();
            
            // Fuer BaseService: CVM -> DM, DM <-> VM

            this.CreateMap<StepCreateViewModel, RecipeStep>();
            this.CreateMap<RecipeStep, StepViewModel>();
            this.CreateMap<StepViewModel, RecipeStep> ();

            this.CreateMap<string, RecipeTag>().ConstructUsing(x => new RecipeTag() {Name = x});
            this.CreateMap<RecipeTag, TagViewModel>();
            this.CreateMap<TagViewModel, RecipeTag>();
        }
    }
}
