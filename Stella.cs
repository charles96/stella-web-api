using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using stella_web_api.Extentions;
using stella_web_api.Models;
using stella_web_api.Repositories;

namespace stella_web_api
{
    public static class Stella
    {
        /// <summary>
        /// ����� ����
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("CompetitorReadAsync")]
        public static async Task<HttpResponseMessage> CompetitorReadAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "competitor/read")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("CompetitorReadAsync request");

            var ret = new CompetitorResponse();

            try
            {
                var res = await StellaRepository.CompetitorReadAsync();

                ret.code = 1;
                ret.data = new Competitor()
                {
                    luna_email = res.luna_email,
                    reg_dt = res.reg_dt,
                    content = res.content
                };
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

            return ret.ObjectToHttpMessage();
        }


        /// <summary>
        /// Īģ�ϱ� - �ۼ�
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ComplimentWriteRunAsync")]
        public static async Task<HttpResponseMessage> ComplimentWriteRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "compliment/write")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("ComplimentWriteRunAsync request..");

            var ret = new ComplimentResponse();

            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

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

            return ret.ObjectToHttpMessage();
        }


        /// <summary>
        /// Īģ�ϱ� - �б�
        /// </summary>
        /// <param name="req"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        [FunctionName("ComplimentReadRunAsync")]
        public static async Task<IActionResult> ComplimentReadRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "compliment/read")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request..");

            string name = req.Query["name"];

            var res = await StellaRepository.ComplimentReadAsync();

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed succe44ssfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {res.from_luna_email}. This HTTP triggered function executed successfully.";

            return new OkObjectResult(responseMessage);
        }

        [FunctionName("GetStatisticAlimtalkMonthlyRunAsync")]
        public static async Task<HttpResponseMessage> GetStatisticAlimtalkMonthlyRunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "statistics/alimtalk/monthly")] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("GetStatisticAlimtalkMonthlyRunAsync request..");

            GetStatisticAlimtalkMonthlyResponse ret = new GetStatisticAlimtalkMonthlyResponse();

            string month = req.Query["month"];

            try
            {
                var inputMonth = Convert.ToDateTime(month);
                var prevMonth = inputMonth.AddMonths(-1);

                var response = await StellaRepository.GetStatisticsAlimtalkMonthlyAsync(prevMonth.ToString("yyyyMM"), inputMonth.ToString("yyyyMM"));

                if (response.Count() > 0)
                {
                    var preCount = (from item in response
                                    where item.send_month.Equals(prevMonth.ToString("yyyyMM"))
                                    let total = item.api_send_count + item.auto_send_count + item.manual_send_count
                                    select total).FirstOrDefault();

                    ret = (from item in response
                           where item.send_month.Equals(inputMonth.ToString("yyyyMM"))
                           let total = item.api_send_count + item.auto_send_count + item.manual_send_count
                           select new GetStatisticAlimtalkMonthlyResponse()
                           {
                               Data = new StatisticAlimtalkMonthly()
                               {
                                   ApiSendCount = item.api_send_count,
                                   AutoSendCount = item.auto_send_count,
                                   ManualSendCount = item.manual_send_count,
                                   TotalSendCount = total,
                                   Increase = (((float)total - (float)preCount) / (float)total) * 100
                               },
                               code = 1
                           }).FirstOrDefault();
                }
                else
                {
                    ret.code = 2;
                }

            }
            catch (FormatException ex)
            {
                log.LogError($"GetStatisticAlimtalkMonthlyRunAsync {ex.Message}");
                ret.code = 3;
            }


            return ret.ObjectToHttpMessage();
        }
    }
}

