using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HIS.Bot.WebApi.Extensions
{
    public static class HttpUtilityExtensions
    {
        public static string ConvertToQueryString<T>(this HttpUtility util, string prefix, T model) where T:class
        {
            const string tempBaseUrl = "http://example.com/";

            if (model == null) { return ""; }

            var builder = new UriBuilder(tempBaseUrl) {Port = -1};
            var query = HttpUtility.ParseQueryString(builder.Query);
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var key = !String.IsNullOrWhiteSpace(prefix) ? $"{prefix}[{propertyInfo.Name}]" : propertyInfo.Name;
                var value = (propertyInfo.GetValue(model) ?? "").ToString();
                query.Add(key, value);
            }

            builder.Query = query.ToString();
            return builder.ToString().Replace(tempBaseUrl, "");
        }

    }
}