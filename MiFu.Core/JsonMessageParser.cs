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
using System.Dynamic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiFu.Core
{
    public class JsonMessageParser : IMiFuMessageParser<string>
    {
        public ExpandoObject DeserializeExpando(string message)
        {
            return ToExpando(message);
        }

        public static ExpandoObject ToExpando(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            ExpandoObject target = JsonConvert.DeserializeObject<ExpandoObject>(message, settings);
            return target;
        }

        public TMessage Deserialize<TMessage>(string message)
        {
            if (message == null)
                throw new ArgumentNullException(nameof(message));

            TMessage target = JsonConvert.DeserializeObject<TMessage>(message, settings);
            return target;
        }

        public string Serialize(ExpandoObject message)
        {
            var json = JsonConvert.SerializeObject(message, settings);
            return json;
        }

        private static readonly JsonSerializerSettings settings =
            new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };
    }
}
