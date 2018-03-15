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
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using MiFu.Core;

namespace MiFu.Chassis.AzFunc1.Transport
{
    public class HttpImpl
    {
        public HttpImpl(/*TraceWriter log*/)
        {

        }

        /// <summary>
        /// Sends (POSTs) the passed in JSON-based message over http to the passed-in target url.
        /// </summary>
        /// <param name="message">The JSON-based message to send.</param>
        /// <param name="targetUrl">The url to POST the message to.</param>
        /// <returns>The message, as sent, and an error string (in the failure case).</returns>
        public async Task<Result2<string>> Send(string message, string targetUrl)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));
            if (string.IsNullOrWhiteSpace(targetUrl))
                throw new ArgumentOutOfRangeException(
                    "Must include one or more non-whitespace characters!", nameof(targetUrl));

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
                // TODO: Log this properly!
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
    }
}
