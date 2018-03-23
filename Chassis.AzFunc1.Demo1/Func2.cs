// Copyright 2018 Jason W. Weber
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.using System;

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
            dynamic message = JsonConvert.DeserializeObject(jsonContent);

            if (message.value1 == null)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, new
                {
                    error = "Please include the value1 property in the message."
                });
            }

            // do some stuff e.g. adding 5 to value1
            message.value2 = 5;
            message.total = message.value1 + 5;

            string jsonOut = JsonConvert.SerializeObject(message);
            return req.CreateResponse(HttpStatusCode.OK, (object)message);
        }
    }
}
