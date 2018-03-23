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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MiFu.Core;
using ch = MiFu.Chassis.AzFunc1;

namespace Chassis.AzFunc1.Demo1
{
    public class DemoConfig : ch.IAzFuncConfig
    {
        public Result2<string> GetRequiredValue(string name, string format = null)
        {
            return GetRequiredValue(name);
        }

        public Result2<string> GetRequiredValue(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentOutOfRangeException(nameof(name), "Can't be null or empty.");

            var address = "http://localhost:7071/api/" + name;
            return Result2<string>.OK(address);
        }
    }
}
