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

using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;

using ch = MiFu.Chassis.AzFunc1;
using co = MiFu.Core;
using m = Chassis.AzFunc1.Demo1.Messages;

namespace Chassis.AzFunc1.Demo1
{
    public static class Func1
    {
        [FunctionName("Func1")]
        public static async Task<object> Run([HttpTrigger(WebHookType = "genericJson")]HttpRequestMessage req, TraceWriter log)
        {
            log.Info($"Webhook was triggered!");

            string jsonContent = await req.Content.ReadAsStringAsync();
            //dynamic data = JsonConvert.DeserializeObject(jsonContent);

            //if (data.first == null || data.last == null)
            //{
            //    return req.CreateResponse(HttpStatusCode.BadRequest, new
            //    {
            //        error = "Please pass first/last properties in the input object"
            //    });
            //}

            // set up the activation context
            ch.IAzFuncConfig config = new DemoConfig();
            ch.IAzFuncLogger logAdapter = new ch.AzFuncLogger(log);
            ch.IAzFuncTransport transport = new ch.AzFuncTransport(logAdapter, config);
            ch.IAzFuncActivationContext context = new ch.AzFuncActivationContext(jsonContent, transport);

            // send the message to the service
            try
            {
                var parser = new co.JsonMessageParser();
                var messageIn = parser.Deserialize<m.Func1MessageIn>(jsonContent);

                var demoService = new Service1();
                var result = await demoService.ActivateAsync(context);

                //var messageOut = await 

                //var messageIn = transport.
                //var messageOut = demoService.Activate(context);
                // NOTE: Convert the messageOut into an appropriate response message here, if any.
                return req.CreateResponse(HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                log.Error("Something went wrong!", ex);
                return req.CreateResponse(HttpStatusCode.InternalServerError, new
                {
                    // NOTE: Don't do this for PROD, it's not secure.
                    ex.Message,
                    ex.StackTrace
                });
            }
        }
    }
}
