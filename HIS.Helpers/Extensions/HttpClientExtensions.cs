using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;

namespace HIS.Helpers.Extensions
{
    public static class HttpClientExtensions
    {
        public static string AddToUrlAsQueryString<T>(this HttpClient client, string baseUrl, string prefix, T model) where T : class
        {
            if (model == null) { return baseUrl; }

            var prop = new Dictionary<string, string>();
            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var key = !String.IsNullOrWhiteSpace(prefix)? $"{prefix}[{propertyInfo.Name}]":propertyInfo.Name;
                var value = (propertyInfo.GetValue(model) ?? "").ToString();
                prop.Add(key, value);
            }
            
            return QueryHelpers.AddQueryString(baseUrl, prop);
        }

    }
}
