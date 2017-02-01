using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using HIS.Bot.WebApi.ConversationFlows.Dialogs;
using HIS.Bot.WebApi.Data.Enums;
using HIS.Bot.WebApi.Extensions;
using Microsoft.Bot.Connector;

namespace HIS.Bot.WebApi.Data.ViewModels
{
    public class ShortRecipeViewModel : NamedViewModel
    {

        #region CONST

        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        public ShortRecipeViewModel()
        {
            Tags = new List<string>();
        }

        #endregion

        #region METHODS
        public Attachment ToAttachment(string buttonValue)
        {
            var action = new CardAction(CardActionTypes.postBack.GetName(), Resource.ButtonLabel_StartCooking, value: buttonValue);
            var image = new CardImage(this.ImageUrl, this.Name);
            return new ThumbnailCard(this.Name, String.Join(", ", this.Tags), images: new List<CardImage>() { image }, buttons: new List<CardAction>() { action }).ToAttachment();
        }

        #endregion

        #region PROPERTIES
        [Required]
        public string Creator { get; set; }
        public string ImageUrl { get; set; }
        public DateTime LastTimeCooked { get; set; }
        public List<string> Tags { get; set; }

        #endregion
    }
}