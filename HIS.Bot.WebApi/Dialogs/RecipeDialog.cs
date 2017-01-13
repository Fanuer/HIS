using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HIS.Bot.WebApi.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace HIS.Bot.WebApi.Dialogs
{
    [LuisModel("f552a909-c8c6-4a0f-ab3b-2a4d4a73af12", "f6b8780c92d14959ab92ca0ce36461ce")]
    [Serializable]
    public class RecipeDialog: LuisDialog<object>
    {
        #region CONST

        private const string Entity_Recipename = "RecipeName";
        private const string Entity_Recipedirection = "RecipeDirection";
        private const string Entity_Tag = "Tag";
        private const string Entity_Ingrediant = "Ingrediant";
        #endregion
        
        #region FIELDS
        #endregion
        
        #region CTOR

        public RecipeDialog()
        {
            
        }
        #endregion

        #region METHODS
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Verstehe ich nicht");
            context.Wait(MessageReceived);
        }

        [LuisIntent("SearchRecipe")]
        public async Task SearchRecipe(IDialogContext context, LuisResult result)
        {
            EntityRecommendation recipeName;
            if (!result.TryFindEntity(Entity_Recipename, out recipeName))
            {
                recipeName = new EntityRecommendation(type: Entity_Recipename) { Entity = "" };
            }
            
            IList<EntityRecommendation> ingrediants;
            if (!result.TryFindEntity(Entity_Ingrediant, out ingrediants))
            {
                ingrediants = new List<EntityRecommendation>();
            }

            IList<EntityRecommendation> tags;
            if (!result.TryFindEntity(Entity_Tag, out tags))
            {
                tags = new List<EntityRecommendation>();
            }

            var markedUpRecipeName = String.IsNullOrWhiteSpace(recipeName.Entity) ? "**keine Angabe**": $"*{recipeName.Entity}*" ;
            var markedUpTags = tags== null || !tags.Any() ? "**keine Angabe**": $"*{String.Join(", ", tags.Select(x => x.Entity))}*" ;
            var markedUpIngrediants = ingrediants == null || !ingrediants.Any() ? "**keine Angabe**": $"*{String.Join(", ", ingrediants.Select(x => x.Entity))}*" ;
            

            var reply = $"Folgende Suchoptionen wurden gefunden: \n\n" +
                         $"Gesuchter Rezeptname: \t{markedUpRecipeName}\n\n" +
                         $"Gesuchte Tags: \t{markedUpTags} \n\n" +
                         $"Gesuchte Zutaten: \t{markedUpIngrediants}";

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Markdown;
            message.Text = reply;
            await context.PostAsync(message);
            context.Wait(MessageReceived);
        }

        [LuisIntent("GetRecipeStep")]
        public async Task GetRecipeStep(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Verstehe ich nicht");
            context.Wait(MessageReceived);
        }


        
        #endregion

        #region PROPERTIES
        #endregion

    }
}
