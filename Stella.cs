using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using stella_web_api.Repositories;

namespace stella_web_api
{
    public static class Stella
    {
        [FunctionName("ComplimentWriteRunAsync")]
        public static async Task<IActionResult> ComplimentWriteRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "compliment/Write")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request..");

            string name = req.Query["name"];

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;


            StellaRepository stellaRepository = new StellaRepository();
            var res = await stellaRepository.ComplimentAsync();

            var count = res.Count().ToString();

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed succe44ssfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello {count}, {name}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
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

