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

namespace MiFu.Chassis.AzFunc
{
    public class AzFuncChassis : MiFuTransportBase<string, object>, IAzFuncChassis
    {
        public AzFuncChassis(IAzFuncConfig config, IMiFuMessageParser<string> parser, IMiFuRegistry<object> registry) :
            base(parser, registry)
        {
            Config = config ?? throw new ArgumentNullException(nameof(config));
        }

        protected override Task<Result2<int>> DoPostAsync(string message, ExpandoObject message2, object[] receivers)
        {
            // NOTE: Receivers are ignored for post, since the queue handles subscribers.

            // get the target topic queue
            const string fieldName = "topic";
            var fields = ((IDictionary<string, object>)message2);
            if (!fields.ContainsKey(fieldName))
            {
                var msg = $"No '{fieldName}' field present in message, cannot be posted.";
                // TODO: Log this proerly!
                Debug.WriteLine(msg);
                return Task.FromResult<Result2<int>>(Result2<int>.Fail(msg));
            }

            // TODO: Send the message to the quee!

            return Task.FromResult<Result2<int>>(Result2<int>.OK(1));
        }

        protected override async Task<Result2<string>> DoSendAsync(string message, ExpandoObject message2, object[] receivers)
        {
            if (receivers.Length > 1)
                throw new NotImplementedException($"{receivers.Length} matches, cannot currently send to multiple receivers.");

            // lookup the recipient's address
            var lookupResult = GetSendRecipient(message2);
            if (!lookupResult.IsOK)
                return Result2<string>.Fail(message, lookupResult.Error);
            var targetUrl = lookupResult.Value;

            // ship it!
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
                Debug.WriteLine(msg);
                return Result2<string>.Fail(msg);
            }
            catch (TaskCanceledException /*ex*/)
            {
                // TODO: Log this proerly!
                var msg = $"Send message timed out";
                Debug.WriteLine(msg);
                return Result2<string>.Fail(msg);
            }
        }

        /// <summary>
        /// Obtains target url based upon mapping target field in message to environment variable.
        /// NOTE: Begs for Consul integration or something similar.
        /// </summary>
        /// <remarks>TODO: Consider if/how to factor more of this into base class/differently.</remarks>
        /// <param name="message"></param>
        /// <returns></returns>
        public Result2<string> GetSendRecipient(ExpandoObject message)
        {
            const string fieldName = "target";
            var fields = ((IDictionary<string, object>)message);
            if (!fields.ContainsKey(fieldName))
                return Result2<string>.Fail($"Required '{fieldName}' field missing in messsage.");

            var key = (string)fields[fieldName];
            // NOTE: Any key transforms should via the injected configuration reader.
            var found = Config.Get(key);
            return found;
        }

        protected IAzFuncConfig Config { get; private set; }
    }
}
