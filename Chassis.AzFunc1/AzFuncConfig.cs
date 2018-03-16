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
using MiFu.Core;

namespace MiFu.Chassis.AzFunc1
{
    public class AzFuncConfig : IAzFuncConfig
    {
        public Result2<string> Get(string key)
        {
            // see https://social.msdn.microsoft.com/Forums/azure/en-US/06afdcb6-349b-4229-b72c-57617705232f/reading-application-settings-from-azure-function?forum=AzureFunctions
            // for how to access environment variables in Azure Functions.
            // NOTE: This appears to work when running locally, too--nice!
            var environmentKey = $"SERVICE-URL-{key}".ToUpper();
            var targetUrl = Environment.GetEnvironmentVariable(environmentKey, EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(targetUrl))
                return Result2<string>.Fail($"Required '{environmentKey}' environment variable missing.");

            return Result2<string>.OK(targetUrl);
        }
    }
}
