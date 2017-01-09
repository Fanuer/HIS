using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Description;
using Autofac;
using Autofac.Core.Lifetime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using Microsoft.Rest;

namespace HIS.Bot.WebApi.Controllers
{
    //[BotAuthentication]
    [Route("api/[controller]")]
    public class MessagesController : Controller
    {
        #region CONST
        #endregion

        #region FIELDS
        #endregion

        #region CTOR
        #endregion

        #region METHODS  

        /// <summary>
        /// Receive a message from a user and reply to it
        /// </summary>

        [ResponseType(typeof(void))]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Activity activity, CancellationToken token)
        {
            if (activity == null) { throw new ArgumentNullException(nameof(activity));}

            var type = activity.GetActivityType();

            if (type.Equals(ActivityTypes.Message))
            {
                // calculate something for us to return
                int length = (activity.Text ?? string.Empty).Length;

                // return our reply to the user
                var reply = activity.CreateReply($"You sent {length} characters");
                var connector = new ConnectorClient(new Uri(activity.ServiceUrl));
                var peng = await connector.Conversations.ReplyToActivityWithHttpMessagesAsync(activity.Conversation.Id, activity.Id, reply, cancellationToken: token);
                
            }
            else
            {
                HandleSystemMessage(activity);
            }
            return Ok();
        }

        private IActionResult HandleSystemMessage(Activity activity)
        {
            switch (activity.GetActivityType())
            {
                case ActivityTypes.Ping:                    // An activity sent to test the security of a bot.
                    var reply = activity.CreateReply();
                    reply.Type = ActivityTypes.Ping;

                    return Ok();
                    break;
                case ActivityTypes.ContactRelationUpdate:   // The bot was added to or removed from a user's contact list
                    break;
                case ActivityTypes.ConversationUpdate:      // This notification is sent when the conversation's properties change, for example the topic name, or when user joins or leaves the group
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

            return null;
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}