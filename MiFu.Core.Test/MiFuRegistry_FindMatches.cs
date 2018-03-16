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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using t = MiFu.Core;
using Moq;

namespace MiFu.Core1.Test
{
    [TestClass]
    public class MiFuRegistry_Find
    {
        [TestMethod]
        public void MiFuRegistry_FindMatches_When1FieldSameValue()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a" });
            var result = test.FindMatches("{\"field1\":\"a\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_NotWhen1FieldDifferentValue()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a" });
            var result = test.FindMatches("{\"field1\":\"b\"}");
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_NotWhen1FieldDifferentNames()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a" });
            var result = test.FindMatches("{\"field2\":\"a\"}");
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_When1FieldWildcard()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:*" });
            var result = test.FindMatches("{\"field1\":\"a\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_When2FieldsSameValue()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a", "field2:b" });
            var result = test.FindMatches("{\"field1\":\"a\",\"field2\":\"b\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }


        [TestMethod]
        public void MiFuRegistry_FindMatches_NotWhen2FieldsOnlyFirstValueSame()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a", "field2:b" });
            var result = test.FindMatches("{\"field1\":\"a\",\"field2\":\"c\"}");
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_NotWhen2FieldsOnlySecondValueSame()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a", "field2:b" });
            var result = test.FindMatches("{\"field1\":\"x\",\"field2\":\"b\"}");
            Assert.IsTrue(result.Length == 0);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_When2FieldsWildcardAndSameValue()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:*", "field2:b" });
            var result = test.FindMatches("{\"field1\":\"a\",\"field2\":\"b\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_When2FieldsSameValueAndWildcard()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:a", "field2:*" });
            var result = test.FindMatches("{\"field1\":\"a\",\"field2\":\"b\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }

        [TestMethod]
        public void MiFuRegistry_FindMatches_When2FieldsBothWildcards()
        {
            var testProxy = new Mock<t.IMiFuService<string>>(MockBehavior.Strict).Object;
            var test = new t.MiFuRegistry<string>();
            test.Register(testProxy, new string[] { "field1:*", "field2:*" });
            var result = test.FindMatches("{\"field1\":\"a\",\"field2\":\"b\"}");
            Assert.IsTrue(result.Length == 1 && result[0] == testProxy);
        }
    }
}
