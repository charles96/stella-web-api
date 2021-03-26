using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace stella_web_api.Extentions
{
    public static class HttpExtention
    {
        public static HttpResponseMessage ObjectToHttpMessage(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj, Formatting.Indented);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }
    }
}
