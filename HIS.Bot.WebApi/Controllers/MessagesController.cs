using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using HIS.Bot.WebApi.Dialogs;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace HIS.Bot.WebApi.Controllers
{
    [BotAuthentication]
    //[Route("api/[controller]")]
    public class MessagesController : ApiController
    {

        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR

        public MessagesController()
        {
        }
        #endregion

        #region METHODS  

        [HttpGet]
        public IHttpActionResult Test()
        {
            return this.Ok();
        }
        /// <summary>
        /// Receive a message from a user and reply to it
        /// </summary>
        [ResponseType(typeof(void))]
        [HttpPost]
        public async Task<HttpResponseMessage> Post([FromBody] Activity activity)
        {
            try
            {
                if (activity == null) { throw new ArgumentNullException(nameof(activity)); }

                var type = activity.GetActivityType();

                if (type.Equals(ActivityTypes.Message))
                {
                    await Conversation.SendAsync(activity, () => new RecipeDialog(), new CancellationToken());
                }
                else
                {
                    await HandleSystemMessage(activity, new CancellationToken());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                this.ModelState.AddModelError("", e.Message);
            }
            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }

        private async Task HandleSystemMessage(Activity incoming, CancellationToken token)
        {
            switch (incoming.GetActivityType())
            {
                case ActivityTypes.Ping:                    // An incoming sent to test the security of a bot.
                    var reply = incoming.CreateReply();
                    reply.Type = ActivityTypes.Ping;
                    await SendReplyMessage(incoming, reply, token);
                    break;
                case ActivityTypes.ContactRelationUpdate:   // The bot was added to or removed from a user's contact list
                    if (incoming.AsContactRelationUpdateActivity().Action.Equals("add"))
                    {
                        await SendReplyMessage(Resource.Message_Welcome, incoming, token);
                    }
                    break;
                case ActivityTypes.ConversationUpdate:      // This notification is sent when the conversation's properties change, for example the topic name, or when user joins or leaves the group
                    if (incoming.AsConversationUpdateActivity().MembersAdded.Any())
                    {
                        await SendReplyMessage(Resource.Message_Welcome, incoming, token);
                    }
                    break;
                case ActivityTypes.DeleteUserData:          // A user has requested for the bot to delete any profile / user data
                    break;
                case ActivityTypes.EndOfConversation:       // End a conversation
                    break;
                case ActivityTypes.Trigger:                 // External system has triggered
                    break;
                case ActivityTypes.Typing:                  // The user or bot on the other end of the conversation is typing
                    break;
            }
        }

        private async Task SendReplyMessage(string text, Activity activity, CancellationToken token = default(CancellationToken))
        {
            var reply = activity.CreateReply(text);
            await SendReplyMessage(activity, reply, token);
        }

        private async Task SendReplyMessage(Activity incomingAct, Activity outgoingAct, CancellationToken token = default(CancellationToken))
        {
            using (var connector = new ConnectorClient(new Uri(incomingAct.ServiceUrl)))
            {
                await connector.Conversations.ReplyToActivityAsync(outgoingAct, token);
            }
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}