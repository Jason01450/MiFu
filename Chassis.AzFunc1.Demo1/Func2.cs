using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

namespace Chassis.AzFunc1.Demo1
{
    public static class Func2
    {
        [FunctionName("Func2")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            dynamic data = JsonConvert.DeserializeObject(jsonContent);

            if (data.first == null || data.last == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = "Please pass first/last properties in the input object"
                });
            }

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                greeting = $"Hello {data.first} {data.last}!"
            });
        }
    }
}
