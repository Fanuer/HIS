using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
using HIS.Bot.WebApi.Properties;

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
        public const string RecipeStartRaw = "\r\n\r\n type 'start cooking ";
        public const string RecipeStartRegEx = RecipeStartRaw + "\\d+";
        private const string RecipeStartStringFormat = RecipeStartRaw + "{0}'";

        #endregion

        #region FIELDS

        #endregion

        #region CTOR

        public RecipeDialog()
        {
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo("de-de");
        }
        #endregion

        #region METHODS

        #region Intents
        [LuisIntent("")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync(Resources.Message_Error_DontUnderstand);
            context.Wait(MessageReceived);
        }

        [LuisIntent("SearchRecipe")]
        public async Task SearchRecipe(IDialogContext context, LuisResult result)
        {
            try
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
                    await context.PostAsync(Resources.Message_RecipesFound);
                    reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;
                    foreach (var shortRecipeViewModel in recipes.Entries)
                    {
                        var attachment = shortRecipeViewModel.ToAttachment(String.Format(RecipeStartStringFormat, shortRecipeViewModel.Id));
                        reply.Attachments.Add(attachment);
                    }

                    //recipes.Entries.Select(x => x.ToAttachment(String.Format(RecipeStartStringFormat, x.Id))).ForEach(x => reply.Attachments.Add(x));
                    await context.PostAsync(reply);
                }
                else
                {
                    await context.PostAsync(Resources.Message_Error_NoRecipesFound);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                await context.PostAsync(Resources.Message_Error_SearchRecipeError);
            }
            

            context.Wait(MessageReceived);
        }


        [LuisIntent("GetNextRecipeStep")]
        public async Task GetNextRecipeStep(IDialogContext context, LuisResult result)
        {
            try
            {
                await this.HandleGetRecipeStep(context, StepDirection.Next);
            }
            catch (RecipeIdNotFoundException e)
            {
                Console.WriteLine(e);
                await context.PostAsync(Resources.Message_Error_NoRecipePicked);
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetLastRecipeStep")]
        public async Task GetLastRecipeStep(IDialogContext context, LuisResult result)
        {
            try
            {
                await this.HandleGetRecipeStep(context, StepDirection.Previous);
            }
            catch (RecipeIdNotFoundException e)
            {
                Console.WriteLine(e);
                await context.PostAsync(Resources.Message_Error_NoRecipePicked);
            }

            context.Wait(MessageReceived);
        }

        [LuisIntent("GetCurrentRecipeStep")]
        public async Task GetCurrentRecipeStep(IDialogContext context, LuisResult result)
        {
            try
            {
                await this.HandleGetRecipeStep(context, StepDirection.ThisStep);
            }
            catch (RecipeIdNotFoundException e)
            {
                Console.WriteLine(e);
                await context.PostAsync(Resources.Message_Error_NoRecipePicked);
            }

            context.Wait(MessageReceived);
        }


        [LuisIntent("GetIngrediantList")]
        public async Task GetIngrediantList(IDialogContext context, LuisResult result)
        {
            int recipeId = -1;
            if (!context.UserData.TryGetValue(Userdata_RecipeId, out recipeId))
            {
                await context.PostAsync(Resources.Message_Error_NoRecipeIdFoundForIngrediantslist);
            }
            else
            {
                var responseText = await this.GetIngrediantListAsync(recipeId);
                await context.PostAsync(responseText);
            }

            context.Wait(MessageReceived);
        }

        #endregion

        internal static IDialog<RecipeSearch> CreateRecipeSearchDialog()
        {
            return Chain.From(() => FormDialog.FromForm(RecipeSearch.BuildForm));
        }

        private async Task HandleGetRecipeStep(IDialogContext context, StepDirection direction)
        {
            try
            {
                int recipeId = -1;
                StepViewModel step = null;
                var stepId = 0;

                if (!context.UserData.TryGetValue(Userdata_RecipeId, out recipeId))
                {
                    throw new RecipeIdNotFoundException();
                }

                if (!context.UserData.TryGetValue(Userdata_StepId, out stepId))
                {
                    stepId = -1;
                }

                using (var client = new GatewayClient())
                {
                    step = await client.GetStepAsync(recipeId, stepId, direction);
                }

                if (step == null)
                {
                    await context.PostAsync(Resources.Message_Error_UnableToReceiveStep);
                }
                else
                {
                    context.UserData.SetValue(Userdata_StepId, step.Id);
                    await context.PostAsync($"{Resources.Message_Step} {step.Order + 1} {Resources.Message_of} {step.TotalSteps} {Resources.Message_Steps}:{Bot_NewLine}{step.Description}");
                    if (step.Order + 1 >= step.TotalSteps)
                    {
                        await context.PostAsync(Resources.Message_CookingCompleted);
                    }
                }
            }
            catch (RecipeIdNotFoundException e)
            {
                await context.PostAsync(Resources.Message_Error_RecipeMustBeSelectedFirst);
            }
        }

        protected override async Task MessageReceived(IDialogContext context, IAwaitable<IMessageActivity> item)
        {
            var activity = await item;
            if (Regex.IsMatch(activity.Text, RecipeStartRegEx))
            {
                try
                {
                    int recipeId = -1;
                    var recipeIdMatch = Regex.Match(activity.Text.Remove(0, RecipeStartRaw.Length), "\\d+");
                    if (!Int32.TryParse(recipeIdMatch.Value, out recipeId))
                    {
                        await context.PostAsync(Resources.Message_Error_RecipeNotFound);
                    }
                    context.UserData.SetValue(Userdata_RecipeId, recipeId);
                    context.UserData.RemoveValue(Userdata_StepId);

                    using (var client = new GatewayClient())
                    {
                        await client.StartCookingAsync(recipeId);
                    }

                    await context.PostAsync(Resources.Message_StartCooking_HereWeGo + Bot_NewLine + Resources.Message_StartCooking_Desc);

                    var text = await GetIngrediantListAsync(recipeId);
                    await context.PostAsync(text);
                    await this.HandleGetRecipeStep(context, StepDirection.ThisStep);
                    
                }
                catch (Exception e)
                {
                    await context.PostAsync("Es ist ein Fehler aufgetreten. Bitte versuch es später nochmal");
                }
                context.Wait(this.MessageReceived);
            }
            else
            {
                await base.MessageReceived(context, item);
            }
        }

        private async Task<string> GetIngrediantListAsync(int recipeId)
        {
            IEnumerable<RecipeIngrediantViewModel> ingrediants = null;
            using (var client = new GatewayClient())
            {
                ingrediants = await client.GetRecipeIngrediantsAsync(recipeId);
            }
            var builder = new StringBuilder();
            builder.AppendLine(Resources.Message_YouNeedFollingIngrediants + Bot_NewLine + Bot_NewLine);
            foreach (var ingrediant in ingrediants.OrderByDescending(x=>x.Amount))
            {
                var line = $"* {ingrediant.Amount.ToString().PadLeft(4)} {ingrediant.Unit.GetUnit().PadRight(3, ' ')} {ingrediant.Name}";
                builder.AppendLine(line);
            }
            return builder.ToString();

        }

        private RecipeSearchViewModel CreateSearchModel(LuisResult result)
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

            var searchModel = new RecipeSearchViewModel()
            {
                Tags = tags.Where(x => !String.IsNullOrWhiteSpace(x.Entity)).Select(x => x.Entity.Replace(" ", "")).ToList(),
                Ingrediants = ingrediants.Where(x => !String.IsNullOrWhiteSpace(x.Entity)).Select(x => x.Entity.Replace(" ", "")).ToList(),
                Name = recipeName.Entity
            };
            return searchModel;
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}
