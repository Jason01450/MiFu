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
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using co = MiFu.Core;
using m = Chassis.AzFunc1.Demo1.Messages;

namespace Chassis.AzFunc1.Demo1
{
    public class Service1 : co.MiFuServiceBase<string>
    {
        public override async Task<string> ActivateAsync(co.IMiFuActivationContext<string> context)
        {
            var messageIn = parser.Deserialize<m.Func1MessageIn>(context.Message);
            // do some message valdation here
            if (messageIn.Value1 <= 0)
                return "Nope!";

            // compose and send a message to another service!
            dynamic message1 = new ExpandoObject();
            message1.target = "Func2";
            message1.message = "Hello Func2";
            message1.value1 = (double)12;
            var msg1Json = parser.Serialize(message1);
            co.Result2<string> message2 = await context.Transport.SendAsync(msg1Json, message1, null);




            throw new NotImplementedException();
        }

        private static readonly co.JsonMessageParser parser = new co.JsonMessageParser();
    }
}
