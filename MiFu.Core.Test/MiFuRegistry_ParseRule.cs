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

namespace MiFu.Core.Test
{
    [TestClass]
    public class MiFuRegistry_ParseRule
    {
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_NoKey()
        {
            t.MiFuRegistry.ParseRule(":value1");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_NoDelimiter()
        {
            t.MiFuRegistry.ParseRule("key1_value1");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_NoValue()
        {
            t.MiFuRegistry.ParseRule("key1:");
            Assert.Fail();
        }

        [TestMethod]
        public void MiFuRegistry_ParseRule_KeyIsCorrect()
        {
            var result = t.MiFuRegistry.ParseRule("key1:value1");
            Assert.AreEqual<string>("key1", result.Key);
        }

        [TestMethod]
        public void MiFuRegistry_ParseRule_ValueIsCorrect()
        {
            var result = t.MiFuRegistry.ParseRule("key1:value1");
            Assert.AreEqual<string>("value1", result.Value);
        }

        [TestMethod]
        public void MiFuRegistry_ParseRule_KeyIsLowerCased()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1:value1");
            Assert.AreEqual<string>("key1", result.Key);
        }

        [TestMethod]
        public void MiFuRegistry_ParseRule_ValueCaseIsUnchanged()
        {
            var result = t.MiFuRegistry.ParseRule("key1:VaLuE1");
            Assert.AreEqual<string>("VaLuE1", result.Value);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_ExtraTrailingColonFails()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1:value1:");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_ExtraColonAndValueFails()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1:value1:value2");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_WhitespaceBeforeKeyFails()
        {
            var result = t.MiFuRegistry.ParseRule(" KeY1:value1");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_WhitespaceAfterKeyFails()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1 :value1");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_WhitespaceAfterValueFails()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1:value1 ");
            Assert.Fail();
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void MiFuRegistry_ParseRule_WhitespaceBeforeValueFails()
        {
            var result = t.MiFuRegistry.ParseRule("KeY1: value1");
            Assert.Fail();
        }
    }
}
