using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace HIS.Helpers.Extensions
{
    public static class HttpContextExtesions
    {
        public static async Task<T> ReadAsAsync<T>(this HttpContent content)
        {
            T result = default(T);
            var stringcontent = await content.ReadAsStringAsync();
            try
            {
                result = (T)JsonConvert.DeserializeObject(stringcontent, typeof(T));
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
