﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;
using ServerException = HIS.Bot.WebApi.ViewModels.ServerException;

namespace HIS.Bot.WebApi.Extensions
{
    public static class HttpClientExtensions
    {
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