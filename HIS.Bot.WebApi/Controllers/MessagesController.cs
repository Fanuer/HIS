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
                return Ok();
            }
            else
            {
                return HandleSystemMessage(activity);
            }
        }

        private IActionResult HandleSystemMessage(Activity activity)
        {
            switch (activity.GetActivityType())
            {
                case "Ping":
                    var reply = activity.CreateReply();
                    reply.Type = "Ping";

                    return Ok();
                    break;
                case "DeleteUserData":
                    // Implement user deletion here
                    // If we handle user deletion, return a real message
                    break;
                case "BotAddedToConversation":
                    break;
                case "BotRemovedFromConversation":
                    break;
                case "UserAddedToConversation":
                    break;
                case "UserRemovedFromConversation":
                    break;
                case "EndOfConversation":
                    break;
            }

            return null;
        }

        #endregion

        #region PROPERTIES
        #endregion

    }
}