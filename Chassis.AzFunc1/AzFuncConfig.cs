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
        public Result2<string> GetRequiredValue(string name)
        {
            GuardName(name);
            return GetEnvironmentVariable(name);
        }

        public Result2<string> GetRequiredValue(string name, string format = null)
        {
            GuardName(name);
            if (!string.IsNullOrEmpty(format))
                name = string.Format(format, name).ToUpper();
            return GetEnvironmentVariable(name);
        }

        private void GuardName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name),
                    $"Must contain one or more non-whitespace characters.");
        }

        private Result2<string> GetEnvironmentVariable(string name)
        {
            // see https://social.msdn.microsoft.com/Forums/azure/en-US/06afdcb6-349b-4229-b72c-57617705232f/reading-application-settings-from-azure-function?forum=AzureFunctions
            // for how to access environment variables in Azure Functions.
            // NOTE: This appears to work when running locally, too--nice!
            var value = Environment.GetEnvironmentVariable(name, EnvironmentVariableTarget.Process);
            if (string.IsNullOrWhiteSpace(value))
                return Result2<string>.Fail($"Required '{name}' environment variable empty or missing.");

            return Result2<string>.OK(value);
        }
    }
}
