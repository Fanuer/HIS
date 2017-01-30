using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Chronic;
using HIS.Bot.WebApi.Clients;
using HIS.Bot.WebApi.ConversationFlows.Flows;
using HIS.Bot.WebApi.Data.Exceptions;
using HIS.Bot.WebApi.Data.ViewModels;
using HIS.Bot.WebApi.Data.ViewModels.Enum;
using HIS.Bot.WebApi.Extensions;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Microsoft.Bot.Connector;

namespace HIS.Bot.WebApi.ConversationFlows.Dialogs
{
    [LuisModel("f552a909-c8c6-4a0f-ab3b-2a4d4a73af12", "f6b8780c92d14959ab92ca0ce36461ce")]
    [Serializable]
    public class RecipeDialog : LuisDialog<object>
    {
        #region CONST

        private const string Entity_Recipename = "RecipeName";
        private const string Entity_Tag = "Tag";
        private const string Entity_Ingrediant = "Ingrediant";
        private const string Userdata_RecipeId = "RecipeId";
        private const string Userdata_StepId = "StepId";
        private static readonly string Bot_NewLine = Environment.NewLine + Environment.NewLine;
        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        public RecipeDialog()
        {
            
        }
        #endregion

        #region METHODS

        #region Intents
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Resource.Message_Error_DontUnderstand);
            context.Wait(MessageReceived);
        }

        [LuisIntent("SearchRecipe")]
        public async Task SearchRecipe(IDialogContext context, LuisResult result)
        {
            var searchModel = CreateSearchModel(result);

            ListViewModel<ShortRecipeViewModel> recipes = null;
            using (var client = new GatewayClient())
            {
                 recipes = await client.GetRecipes(searchModel);
            }

            IMessageActivity reply = context.MakeMessage();
            reply.Recipient = context.Activity.From;
            reply.Type = ActivityTypes.Message;

            if (recipes.Entries.Any())
            {
                await context.PostAsync(Resource.Message_RecipesFound);
                recipes.Entries.Select(x => x.ToAttachment()).ForEach(x => reply.Attachments.Add(x));
                await context.PostAsync(reply);
            }
            else
            {
                await context.PostAsync(Resource.Message_Error_NoRecipesFound);
            }
            
            context.Wait(MessageReceived);

            //await Conversation.SendAsync(context.Activity as IMessageActivity, CreateRecipeSearchDialog);

            /*
            var markedUpRecipeName = String.IsNullOrWhiteSpace(recipeName.Entity) ? "**keine Angabe**": $"*{recipeName.Entity}*" ;
            var markedUpTags = !tags.Any() ? "**keine Angabe**": $"*{String.Join(", ", tags.Select(x => x.Entity))}*" ;
            var markedUpIngrediants = !ingrediants.Any() ? "**keine Angabe**": $"*{String.Join(", ", ingrediants.Select(x => x.Entity))}*" ;
            

            var reply = $"Folgende Suchoptionen wurden gefunden: \n\n" +
                         $"Gesuchter Rezeptname: \t{markedUpRecipeName}\n\n" +
                         $"Gesuchte Tags: \t{markedUpTags} \n\n" +
                         $"Gesuchte Zutaten: \t{markedUpIngrediants}";

            var message = context.MakeMessage();
            message.TextFormat = TextFormatTypes.Markdown;
            message.Text = reply;
            */
        }

        private RecipeSearchViewModel CreateSearchModel(LuisResult result)
        {
            EntityRecommendation recipeName;
            if (!result.TryFindEntity(Entity_Recipename, out recipeName))
            {
                recipeName = new EntityRecommendation(type: Entity_Recipename) {Entity = ""};
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

            var searchModel = new RecipeSearchViewModel()
            {
                Tags = tags.Where(x=>!String.IsNullOrWhiteSpace(x.Entity)).Select(x => x.Entity.Replace(" ", "")).ToList(),
                Ingrediants = ingrediants.Where(x => !String.IsNullOrWhiteSpace(x.Entity)).Select(x => x.Entity.Replace(" ", "")).ToList(),
                Name = recipeName.Entity
            };
            return searchModel;
        }

        [LuisIntent("GetNextRecipeStep")]
        public async Task GetNextRecipeStep(IDialogContext context, LuisResult result)
        {
            await this.HandleGetRecipeStep(context, StepDirection.Next);
        }

        [LuisIntent("GetLastRecipeStep")]
        public async Task GetLastRecipeStep(IDialogContext context, LuisResult result)
        {
            await this.HandleGetRecipeStep(context, StepDirection.Previous);
        }

        [LuisIntent("GetCurrentRecipeStep")]
        public async Task GetCurrentRecipeStep(IDialogContext context, LuisResult result)
        {
            await this.HandleGetRecipeStep(context, StepDirection.ThisStep);
        }

        private async Task HandleGetRecipeStep(IDialogContext context, StepDirection direction)
        {
            try
            {
                int recipeId = -1;
                if (!context.UserData.TryGetValue(Userdata_RecipeId, out recipeId))
                {
                    throw new RecipeIdNotFoundException();
                }
                var stepId = -1;
                context.UserData.TryGetValue(Userdata_StepId, out stepId);

                StepViewModel step = null;
                using (var client = new GatewayClient())
                {
                    step = await client.GetStepAsync(recipeId, stepId, direction);
                }

                context.UserData.SetValue(Userdata_StepId, step.Id);

                await context.PostAsync(step.Description);
            }
            catch (RecipeIdNotFoundException e)
            {
                await context.PostAsync("Es muss zuerst ein Rezept gewählt werden, das Kochen gestartet werden kann");
            }
            context.Wait(MessageReceived);
        }

        [LuisIntent("GetIngrediantList")]
        public async Task GetIngrediantList(IDialogContext context, LuisResult result)
        {
            int recipeId = -1;
            if (!context.UserData.TryGetValue(Userdata_RecipeId, out recipeId))
            {
                await context.PostAsync(Resource.Message_Error_NoRecipeIdFoundForIngrediantslist);
            }
            else
            {
                var responseText = await this.GetIngredintListTextAsync(recipeId);
                await context.PostAsync(responseText);
            }

            context.Wait(MessageReceived);
        }

        #endregion
        
        internal static IDialog<RecipeSearch> CreateRecipeSearchDialog()
        {
            return Chain.From(() => FormDialog.FromForm(RecipeSearch.BuildForm));
        }

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            try
            {
                var activity = await item;
                if (Regex.IsMatch(activity.Text, ShortRecipeViewModel.RecipeStartRegEx))
                {
                    int recipeId = -1;
                    if (!Int32.TryParse(activity.Text.Remove(0, ShortRecipeViewModel.RecipeStartRaw.Length), out recipeId))
                    {
                        await context.PostAsync(Resource.Message_Error_RecipeNotFound);
                    }
                    context.UserData.SetValue(Userdata_RecipeId, recipeId);
                    context.UserData.RemoveValue(Userdata_StepId);

                    using (var client = new GatewayClient())
                    {
                        await client.StartCookingAsync(recipeId);
                    }

                    await context.PostAsync(Resource.Message_StartCooking_HereWeGo + Bot_NewLine + Resource.Message_StartCooking_Desc);

                    var text = GetIngredintListTextAsync(recipeId);
                    context.Wait(this.MessageReceived);


                }
                else
                {
                    await base.MessageReceived(context, item);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e; 
            }
        }

        private async Task<string> GetIngredintListTextAsync(int recipeId)
        {
            IEnumerable<RecipeIngrediantViewModel> ingrediants = null;
            using (var client = new GatewayClient())
            {
                ingrediants = await client.GetRecipeIngrediantsAsync(recipeId);
            }
            var builder = new StringBuilder();
            builder.AppendLine(Resource.Message_YouNeedFollingIngrediants + Bot_NewLine + Bot_NewLine);
            foreach (var ingrediant in ingrediants)
            {
                builder.AppendLine($"* {ingrediant.Amount.ToString().PadLeft(4, ' ')} {ingrediant.Unit.GetUnit().PadRight(3, ' ')} {ingrediant.Name}{Bot_NewLine}");
            }
            return builder.ToString();

        }
        #endregion

        #region PROPERTIES
        #endregion

    }
}
