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

using System.Configuration;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t = MiFu.Core;
using tc = MiFu.Chassis.AzFunc;

namespace MiFu.Chassis.AzFunc.Test
{
    [TestClass]
    public class UnitTest1
    {
        [Ignore] // temporary
        [TestMethod]
        public async Task TestMethod1()
        {
            var preTest = ConfigurationManager.AppSettings["SERVICE-URL-TEST1"];
            var config = new UnitTestConfig();

            var parser = new t.JsonMessageParser();
            var registry = new t.MiFuRegistry();
            registry.Register("service proxy", new string[] { "field1:hello", "target:test1" });
            var test = new tc.AzFuncTransport(config, parser, registry);
            var result = await test.SendAsync("{\"target\":\"test1\",\"field1\":\"hello\"}");
            Assert.IsTrue(result.IsOK);
        }
    }
}
