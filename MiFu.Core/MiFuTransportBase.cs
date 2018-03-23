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

//using System;
//using System.Dynamic;
//using System.Threading.Tasks;

//namespace MiFu.Core
//{
//    public abstract class MiFuTransportBase<TLog, TConfig, TMessage> : IMiFuTransport<TMessage>
//        where TLog : new() where TConfig: new()
//    {
//        protected MiFuTransportBase(TLog log, TConfig config)
//        {
//            Log = log ?? throw new ArgumentNullException(nameof(log));
//            Config = config ?? throw new ArgumentNullException(nameof(config));
//        }

//        public TConfig Config { get; private set; }

//        public TLog Log { get; private set; }

//        public Task<Result2<int>> PublishAsync(TMessage message, ExpandoObject message2, IMiFuService<TMessage>[] receivers)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<Result2<TMessage>> SendAsync(TMessage message, ExpandoObject message2, IMiFuService<TMessage>[] receivers)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}

//{
//    public abstract class MiFuTransport<TMessage> : IMiFuTransport<TMessage>
//    {
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="parser"></param>
//        /// <param name="registry">(Optinonal) instance of a local registry to use for lookup.</param>
//        public MiFuTransport(IMiFuTransportImpl<TMessage> impl, IMiFuMessageParser<TMessage> parser,
//            IMiFuRegistry<TMessage> registry)
//        {
//            Impl = impl ?? throw new ArgumentNullException(nameof(impl));
//            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
//            Registry = registry; // ?? throw new ArgumentNullException(nameof(registry));
//        }

//        public async Task<Result2<int>> PublishAsync(TMessage message)
//        {
//            ExpandoObject cast = Parser.DeserializeExpando(message);

//            IMiFuService<TMessage>[] matches = NoMatches;
//            if (Registry != null)
//            {
//                matches = Registry.FindMatches(cast);
//                if (matches.Length == 0)
//                    return Result2<int>.OK(0);
//            }

//            var result = await Impl.PublishAsync(message, cast, matches);
//            return result;
//        }

//        public async Task<Result2<TMessage>> SendAsync(TMessage message)
//        {
//            ExpandoObject cast = Parser.DeserializeExpando(message);

//            IMiFuService<TMessage>[] matches = NoMatches;
//            if (Registry != null)
//            {
//                matches = Registry.FindMatches(cast);
//                if (matches.Length == 0)
//                    return Result2<TMessage>.Fail(message, "No matches found.");
//            }

//            var result = await Impl.SendAsync(message, cast, matches);
//            return result;
//        }

//        private IMiFuTransportImpl<TMessage> Impl { get; set; }

//        private static readonly IMiFuService<TMessage>[] NoMatches = new IMiFuService<TMessage>[0];

//        private IMiFuMessageParser<TMessage> Parser { get; set; }

//        private IMiFuRegistry<TMessage> Registry { get; set; }
//    }
//}
