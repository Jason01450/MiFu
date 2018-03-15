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
using System.Threading.Tasks;

namespace MiFu.Core
{
    public abstract class MiFuTransportBase<TMessage, TService> : IMiFuTransport<TMessage>
    {

        protected MiFuTransportBase(IMiFuMessageParser<TMessage> parser, IMiFuRegistry<TService> registry)
        {
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
            Registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        public async Task<Result2<int>> PostAsync(TMessage message)
        {
            ExpandoObject cast = Parser.DeserializeExpando(message);
            TService[] matches = Registry.FindMatches(cast);
            if (matches.Length == 0)
                return Result2<int>.OK(0);

            var result = await DoPostAsync(message, cast, matches);
            return result;
        }

        public async Task<Result2<TMessage>> SendAsync(TMessage message)
        {
            ExpandoObject cast = Parser.DeserializeExpando(message);
            TService[] matches = Registry.FindMatches(cast);
            if (matches.Length == 0)
                return Result2<TMessage>.Fail(message, "No matches found.");

            var result = await DoSendAsync(message, cast, matches);
            return result;
        }

        protected abstract Task<Result2<int>> DoPostAsync(TMessage message, ExpandoObject message2, TService[] receivers);

        protected abstract Task<Result2<TMessage>> DoSendAsync(TMessage message, ExpandoObject message2, TService[] receivers);

        protected IMiFuMessageParser<TMessage> Parser { get; private set; }

        protected IMiFuRegistry<TService> Registry { get; private set; }
    }
}
