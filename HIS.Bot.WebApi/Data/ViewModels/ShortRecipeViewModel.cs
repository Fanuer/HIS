using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Bot.WebApi.Data.Enums;
using HIS.Bot.WebApi.Extensions;
using Microsoft.Bot.Connector;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class ShortRecipeViewModel : NamedViewModel
    {

        #region CONST

        public const string RecipeStartRaw = "start cooking ";
        public const string RecipeStartRegEx ="^" + RecipeStartRaw+ "\\d+$";
        private const string RecipeStartStringFormat = RecipeStartRaw +"{0}";
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS
        #endregion

        #region PROPERTIES
        #endregion


        public ShortRecipeViewModel()
        {
            Tags = new List<string>();
        }
        [Required]
        public string Creator { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public List<string> Tags { get; set; }

        public Attachment ToAttachment()
        {
            var action = new CardAction(CardActionTypes.postBack.GetName(), Resource.ButtonLabel_StartCooking, value:String.Format(RecipeStartStringFormat, Id));
            var image = new CardImage(this.ImageUrl, this.Name);
            return new ThumbnailCard(this.Name, String.Join(", ", this.Tags), images: new List<CardImage>() {image}, buttons:new List<CardAction>() {action}).ToAttachment();
        }
    }
}