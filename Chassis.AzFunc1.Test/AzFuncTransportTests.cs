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
using System.Dynamic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MiFu.Core;
using Moq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using t = MiFu.Chassis.AzFunc1;

namespace MiFu.Chassis.AzFunc1.Test
{
    [TestClass]
    public class AzFuncTransportTests
    {
        private class TestTuple
        {
            public Mock<t.IAzFuncLogger> MockLog = new Mock<t.IAzFuncLogger>(MockBehavior.Strict);

            public Mock<t.IAzFuncConfig> MockConfig = new Mock<t.IAzFuncConfig>(MockBehavior.Strict);

            public t.AzFuncTransport Inst { get; private set; }

            public TestTuple()
            {
                Inst = new t.AzFuncTransport(MockLog.Object, MockConfig.Object);

            }
        }

        [TestMethod]
        public void AzFuncTransportImpl_GetRequiredMessageField_WhenPresent()
        {
            var test = new TestTuple();

            const string testFieldName = "field1";
            const string testFieldValue = "value1";
            var message = new ExpandoObject();
            var fields = message as IDictionary<string, Object>;
            fields.Add(testFieldName, testFieldValue);

            Result2<string> result = test.Inst.GetRequiredMessageField(message, testFieldName, "testing");
            Assert.IsTrue(result.IsOK);
            Assert.IsTrue(result.Value == testFieldValue);
            Assert.AreEqual<string>(string.Empty, result.Error);
        }


        [TestMethod]
        public void AzFuncTransportImpl_GetRequiredMessageField_WhenNotPresent()
        {
            var test = new TestTuple();

            const string testFieldName = "field1";
            const string testFieldValue = "value1";
            var message = new ExpandoObject();
            var fields = message as IDictionary<string, Object>;
            fields.Add(testFieldName, testFieldValue);

            Result2<string> result = test.Inst.GetRequiredMessageField(message, "SomeOtherField", "testing");
            Assert.IsTrue(!result.IsOK);
            Assert.IsTrue(result.Value == null);
            Assert.AreEqual<string>("No 'SomeOtherField' field present in message, cannot be testing.", result.Error);
        }

        [TestMethod]
        public void AzFuncTransportImpl_GetTopicUrlFromConfiguration_ValidTopicName()
        {
            const string testUrl = "http://some-non-working-url.org/topic";
            var test = new TestTuple();
            test.MockConfig.Setup(m => m.GetRequiredValue("needs", "TOPIC-URL-{0}")).Returns(
                Result2<string>.OK(testUrl));

            string message = "{\"topic\":\"needs\",\"someOtherField\":\"some value\"}";
            ExpandoObject message2 = JsonConvert.DeserializeObject<ExpandoObject>(message,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            Result2<string> result = test.Inst.GetTopicUrlFromConfiguration(message2);
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual<string>(testUrl, result.Value);
        }

        [TestMethod]
        public void AzFuncTransportImpl_GetServiceUrlFromConfiguration_ValidServiceName()
        {
            const string testUrl = "http://some-non-working-url.org/service";
            var test = new TestTuple();
            test.MockConfig.Setup(m => m.GetRequiredValue("inquiries", "SERVICE-URL-{0}")).Returns(
                Result2<string>.OK(testUrl));

            string message = "{\"target\":\"inquiries\",\"someOtherField\":\"some value\"}";
            ExpandoObject message2 = JsonConvert.DeserializeObject<ExpandoObject>(message,
                new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });

            Result2<string> result = test.Inst.GetServiceUrlFromConfiguration(message2);
            Assert.IsTrue(result.IsOK);
            Assert.AreEqual<string>(testUrl, result.Value);
        }
    }
}
