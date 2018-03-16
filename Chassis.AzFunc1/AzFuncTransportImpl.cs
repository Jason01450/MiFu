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
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MiFu.Core;

namespace MiFu.Chassis.AzFunc1
{
    public interface IAzFuncTransportImpl : IMiFuTransportImpl<string>
    {
    }

    public class AzFuncTransportImpl : IAzFuncTransportImpl
    {
        public async Task<Result2<int>> PublishAsync(string message, ExpandoObject message2, IMiFuService<string>[] receivers)
        {
            // TODO: Implement the local (dev) case.
            if (receivers.Length > 0)
                throw new System.NotImplementedException();

            // get the target topic queue name
            var topicName = GetRequiredMessageField(message2, "topic", "published");
            if (!topicName.IsOK)
                return Result2<int>.Fail(topicName.Error);

            // get the target topic url from configuration
            var environmentKey = $"TOPIC-URL-{topicName}".ToUpper();
            var topicUrl = GetRequiredConfiguredValue(environmentKey);
            if (!topicUrl.IsOK)
                return Result2<int>.Fail(topicUrl.Error);

            // ship it!
            var result1 = await DoSendAsync(message, topicUrl.Value);
            return result1.IsOK ? Result2<int>.OK(1) : Result2<int>.Fail(result1.Error);
        }

        public async Task<Result2<string>> SendAsync(string message, ExpandoObject message2, IMiFuService<string>[] receivers)
        {
            // TODO: Implement the local (dev) case.
            if (receivers.Length > 0)
                throw new System.NotImplementedException();

            // get the target service name
            var serviceName = GetRequiredMessageField(message2, "target", "sent");
            if (!serviceName.IsOK)
                return Result2<string>.Fail(serviceName.Error);

            // get the target service address from configuration
            var environmentKey = $"SERVICE-URL-{serviceName}".ToUpper();
            var serviceUrl = GetRequiredConfiguredValue(environmentKey);
            if (!serviceUrl.IsOK)
                return Result2<string>.Fail(serviceUrl.Error);

            // ship it!
            var result = await DoSendAsync(message, serviceUrl.Value);
            return result;
        }

        public Result2<string> GetRequiredMessageField(ExpandoObject message2, string fieldName, string errorVerb)
        {
            var fields = ((IDictionary<string, object>)message2);
            if (!fields.ContainsKey(fieldName))
            {
                var msg = $"No '{fieldName}' field present in message, cannot be {errorVerb}.";
                // TODO: Log this proerly!
                Debug.WriteLine(msg);
                return Result2<string>.Fail(msg);
            }

            var fieldValue = (string)fields[fieldName];
            if (string.IsNullOrWhiteSpace(fieldValue))
            {
                var msg = $"The '{fieldName}' field must contain one or more non-whitespace characters.";
                // TODO: Log this proerly!
                Debug.WriteLine(msg);
                return Result2<string>.Fail(msg);
            }

            return Result2<string>.OK(fieldValue);
        }

        public Result2<string> GetRequiredConfiguredValue(string name)
        {
            // see https://social.msdn.microsoft.com/Forums/azure/en-US/06afdcb6-349b-4229-b72c-57617705232f/reading-application-settings-from-azure-function?forum=AzureFunctions
            // for how to access environment variables in Azure Functions.
            // NOTE: This appears to work when running locally, too--nice!
            var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(value))
                return Result2<string>.Fail($"Required '{name}' environment variable empty or missing.");

            return Result2<string>.OK(value);
        }

        public async Task<Result2<string>> DoSendAsync(string message, string targetUrl)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    // send syncronous message to service
                    var content = new StringContent(message, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(targetUrl, content);

                    int code = (int)response.StatusCode;
                    if (code < 200 || code >= 300)
                        return Result2<string>.Fail($"{code}:{response.ReasonPhrase}");

                    var jsonContent = await response.Content.ReadAsStringAsync();
                    return Result2<string>.OK(jsonContent);
                }
            }
            catch (HttpRequestException ex)
            {
                // TODO: Log this proerly!
                var msg = $"{ex.HResult}:{ex.InnerException?.Message ?? "Something went wrong"}";
                Debug.WriteLine("[TRANSPORT]:" + msg);
                return Result2<string>.Fail(msg);
            }
            catch (TaskCanceledException /*ex*/)
            {
                // TODO: Log this proerly!
                var msg = $"Send message timed out";
                Debug.WriteLine("[TRANSPORT]:" + msg);
                return Result2<string>.Fail(msg);
            }
        }
    }
}
