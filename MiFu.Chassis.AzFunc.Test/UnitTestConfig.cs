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

using MiFu.Core;
using System.Collections.Generic;

namespace MiFu.Chassis.AzFunc.Test
{
    public class UnitTestConfig : IAzFuncConfig
    {
        internal UnitTestConfig()
        {
            lookUp.Add("test1", "http://oh-yeah.com");
            lookUp.Add("SERVICE-URL-TEST1", "http://oh-yeah.com");
        }

        private readonly Dictionary<string, string> lookUp = new Dictionary<string, string>();

        public Result2<string> Get(string key)
        {
            if (lookUp.ContainsKey(key))
                return Result2<string>.OK(lookUp[key]);
            else
                return Result2<string>.Fail($"The '{key}' key is not present in the unit test configuration.");
        }
    }
}
