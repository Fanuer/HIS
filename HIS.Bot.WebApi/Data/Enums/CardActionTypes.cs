using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Bot.WebApi.Data.Enums
{
    public enum CardActionTypes
    {
        /// <summary>
        /// URL to be opened in the built-in browser.
        /// </summary>
        openUrl,
        /// <summary>
        /// Text of message which client will sent back to bot as ordinary chat message.All other participants will see that was posted to the bot and who posted this.
        /// </summary>
        imBack,
        /// <summary>
        /// Text of message which client will post to bot.Client applications will not display this message.
        /// </summary>
        postBack,
        /// <summary>
        /// Destination for a call in following format: "tel:123123123123"
        /// </summary>
        call,
        /// <summary>
        /// playback audio container referenced by URL
        /// </summary>
        playAudio,
        /// <summary>
        /// playback video container referenced by URL
        /// </summary>
        playVideo,
        /// <summary>
        /// show image referenced by URL
        /// </summary>
        showImage,
        /// <summary>
        /// download file referenced by URL
        /// </summary>
        downloadFile,
        /// <summary>
        /// OAuth flow URL
        /// </summary>
        signin


    }
}