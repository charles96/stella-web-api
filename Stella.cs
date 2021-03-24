using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using stella_web_api.Models;
using stella_web_api.Repositories;

namespace stella_web_api
{
    public static class Stella
    {

        [FunctionName("ComplimentWriteRunAsync")]
        public static async Task<HttpResponseMessage> ComplimentWriteRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "compliment/write")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request..");

            ComplimentResponse ret = new ComplimentResponse();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            try
            {
                var request = JsonConvert.DeserializeObject<ComplimentRequest>(requestBody);
                var res = await StellaRepository.ComplimentWriteAsync(request);

                if (res >= 1) ret.code = 1;

            }
            catch (MySqlException ex)
            {
                ret.code = 2;
                ret.message = ex.Message;
            }
            catch (Exception ex)
            {
                ret.code = 3;
                ret.message = ex.Message;
            }

            var json = JsonConvert.SerializeObject(ret, Formatting.Indented);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            };
        }

        [FunctionName("ComplimentReadRunAsync")]
        public static async Task<IActionResult> ComplimentReadRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "compliment/read")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request..");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed succe44ssfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }
    }
}

