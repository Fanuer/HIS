using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using HIS.Helpers.Exceptions;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;

namespace HIS.Helpers.Extensions
{
    public static class HttpClientExtensions
    {
        /// <summary>
        /// Converts a 1-deep object to a query string
        /// </summary>
        /// <typeparam name="T">Type of the model</typeparam>
        /// <param name="client">calling client</param>
        /// <param name="model">modeldata to add</param>
        /// <returns></returns>
        public static string ConvertToQueryString<T>(this HttpClient client, T model) where T : class
        {
            if (model == null) { return ""; } 
            var propList = new List<KeyValuePair<string, string>>();

            foreach (var propertyInfo in model.GetType().GetProperties())
            {
                var key = propertyInfo.Name;
                if (propertyInfo.PropertyType != typeof(string) && propertyInfo.PropertyType.GetInterfaces().Contains(typeof(IEnumerable)))
                {
                    var enumerableValue = propertyInfo.GetValue(model) as IEnumerable;
                    if (enumerableValue == null) continue;
                    propList.AddRange(enumerableValue.Cast<object>().Select(x => new KeyValuePair<string, string>(key, x.ToString())));
                }
                else
                {
                    var rawValue = propertyInfo.GetValue(model);
                    var value = (rawValue ?? "").ToString();
                    propList.Add(new KeyValuePair<string, string>(key, value));
                }
            }
            return propList.Any() ? String.Join("&", propList.Select(x => $"{x.Key}={WebUtility.UrlEncode(x.Value)}").ToArray()): "";
        }

        /// <summary>
        /// Adds the models porperties to the given uri as query parameters
        /// </summary>
        /// <typeparam name="T">model type</typeparam>
        /// <param name="client">Http-Client</param>
        /// <param name="uri">given URL</param>
        /// <param name="model">model whose properties will be added</param>
        /// <returns></returns>
        public static string AddToQueryString<T>(this HttpClient client, string uri, T model) where T : class
        {
            // Convert model to query-string
            var queryString = client.ConvertToQueryString(model);
            // if no uri was passed just return the queryString
            if (String.IsNullOrWhiteSpace(uri)) {return queryString;}
            // if no model was passed just return the uri
            if (String.IsNullOrWhiteSpace(queryString)) { return uri;}
            // if the uri doen not contain query parameters add them 
            return !uri.Contains('?') ? $"{uri}?{queryString}" : $"{uri}&{queryString}";
        }

        public static async Task<HttpResponseMessage> PostAsJsonAsync<T>(this HttpClient client, string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await client.PostAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }

        public static async Task<HttpResponseMessage> PutAsJsonAsync<T>(this HttpClient client, string url, T model)
        {
            var json = JsonConvert.SerializeObject(model);
            return await client.PutAsync(url, new StringContent(json, Encoding.UTF8, "text/json"));
        }

        public static async Task<T> GetAsync<T>(this HttpClient client, string url, params object[] args)
        {
            var response = await client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsAsync<T>();
            }

            throw new ServerException(response);
        }

        public static async Task<HttpStatusCode> GetHttpStatusAsync(this HttpClient client, string url, params object[] args)
        {

            var response = await client.GetAsync(String.Format(url, args));
            return response.StatusCode;
        }

        public static async Task<byte[]> GetBytesAsync(this HttpClient client, string url, params object[] args)
        {

            var response = await client.GetAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("GetBytesAsync<{0}>({1}) -> {2}", typeof(byte[]).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsByteArrayAsync();
            }
            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed GetBytesAsync<{0}>({1}) -> {2}{3}", typeof(byte[]).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response));

            throw new ServerException(response);
        }

        public static async Task PutAsJsonAsync<T>(this HttpClient client, T model, string url, params object[] args)
        {

            var response = await client.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled)
                //Log.Debug(String.Format("PutAsJsonAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled)
                //Log.Info(String.Format("Failed PutAsJsonAsync<{0}>({1}) -> {2}{3}", typeof(T).Name,
                //String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        public static async Task PutAsync(this HttpClient client, string url, params object[] args)
        {

            var response = await client.PutAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PutAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        public static async Task DeleteAsync(this HttpClient client, string url, params object[] args)
        {

            var response = await client.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("DeleteAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed DeleteAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        public static async Task<T> DeleteAsync<T>(this HttpClient client, string url, params object[] args)
        {

            var response = await client.DeleteAsync(String.Format(url, args));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("DeleteAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<T>();
            }
            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed DeleteAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
            throw new ServerException(response);
        }

        public static async Task<T> PostAsync<T>(this HttpClient client, HttpContent content, string url, params object[] args)
        {

            var response = await client.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<T>();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PostAsync<{0}>({1}) -> {2}{3}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        public static async Task PostAsync(this HttpClient client, HttpContent content, string url, params object[] args)
        {

            var response = await client.PostAsync(String.Format(url, args), content);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled)
                //Log.Debug(String.Format("PostAsync(HttpContent, {0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
            }
            else
            {
                //if (Log.IsInfoEnabled)
                //Log.Info(String.Format("Failed PostAsync(HttpContent, {0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }

        public static async Task PostAsync(this HttpClient client, string url, params object[] args)
        {

            var response = await client.PostAsync(String.Format(url, args), new StringContent(String.Empty));
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsync({0}) -> {1}", String.Format(url, args), await response.Content.ReadAsStringAsync()));
                //response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync({0}) -> {1}{2}", String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));

                throw new ServerException(response);
            }
        }

        public static async Task PostAsJsonAsync<T>(this HttpClient client, T model, string url, params object[] args)
        {

            var response = await client.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsync<{0}>({1}) -> {2}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                response.Content.Dispose();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync<{0}>({1}) -> {2}{3}", typeof(T).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));

                throw new ServerException(response);
            }
        }

        public static async Task<TResult> PutAsJsonReturnAsync<T, TResult>(this HttpClient client, T model, string url, params object[] args)
        {

            var response = await client.PutAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PutAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));

                return await response.Content.ReadAsAsync<TResult>();
            }

            //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PutAsJsonAsync<{0}, {1}>({2}) -> {3}{4}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
            throw new ServerException(response);
        }

        public static async Task<TResult> PostAsJsonReturnAsync<T, TResult>(this HttpClient client, T model, string url, params object[] args)
        {

            var response = await client.PostAsJsonAsync(String.Format(url, args), model);
            if (response.IsSuccessStatusCode)
            {
                //if (Log.IsDebugEnabled) Log.Debug(String.Format("PostAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync()));
                return await response.Content.ReadAsAsync<TResult>();
            }
            else
            {
                //if (Log.IsInfoEnabled) Log.Info(String.Format("Failed PostAsJsonAsyncReturn<{0}, {1}>({2}) -> {3}{4}", typeof(T).Name, typeof(TResult).Name, String.Format(url, args), await response.Content.ReadAsStringAsync(), response.ToString()));
                throw new ServerException(response);
            }
        }


    }
}
