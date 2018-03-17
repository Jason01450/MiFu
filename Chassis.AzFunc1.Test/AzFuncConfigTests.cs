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
using t = MiFu.Chassis.AzFunc1;

namespace MiFu.Chassis.AzFunc1.Test
{
    [TestClass]
    public class AzFuncConfigTests
    {
        [TestMethod]
        public void AzFuncConfig_GetRequired_KnownPresentVariableFound()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("path");
            Assert.IsTrue(result.IsOK && result.Value.Length > 20);
        }

        [TestMethod]
        public void AzFuncConfig_GetRequired_KnownMissingVariableNotFound()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("path123231safsdfdsf3324weesfsdf");
            Assert.IsTrue(!result.IsOK && result.Value == null);
        }

        [TestMethod]
        public void AzFuncConfig_GetRequired_KnownPresentTemplatedVariableFound()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("pa", "{0}th");
            Assert.IsTrue(result.IsOK && result.Value.Length > 20);
        }

        [TestMethod]
        public void AzFuncConfig_GetRequired_KnownMissingTemplateVariableNotFound()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("pa", "{0}th123231safsdfdsf3324weesfsdf");
            Assert.IsTrue(!result.IsOK && result.Value == null);
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AzFuncConfig_GetRequired_NullPassedIn()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue(null);
            Assert.Fail("Should not get this far.");
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AzFuncConfig_GetRequired_EmptyPassedIn()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue(string.Empty);
            Assert.Fail("Should not get this far.");
        }

        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        [TestMethod]
        public void AzFuncConfig_GetRequired_WhitespacePassedIn()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("    ");
            Assert.Fail("Should not get this far.");
        }

        [ExpectedException(typeof(FormatException))]
        [TestMethod]
        public void AzFuncConfig_GetRequired_InvalidFormatString()
        {
            var test = new t.AzFuncConfig();
            var result = test.GetRequiredValue("path", "{1}-blah!");
            Assert.Fail("Should not get this far.");
        }
    }
}
