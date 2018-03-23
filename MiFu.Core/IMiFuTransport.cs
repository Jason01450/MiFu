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

using System.Dynamic;
using System.Threading.Tasks;

namespace MiFu.Core
{
    public interface IMiFuTransport<TMessage>
    {
        /// <summary>
        /// Publishes a message asynchronously e.g. to a service bus topic. This method ends in Async per the MS
        /// design guidelines for asyncronous methods.
        /// </summary>
        /// <remarks>NOTE: This requires the message to be self-describing with respect to destination.</remarks>
        /// <param name="message"></param>
        /// <param name="message2">The inbound message converted into an ExpandoObject.</param>
        /// <param name="receivers"></param>
        /// <returns></returns>
        Task<Result2<int>> PublishAsync(TMessage message, ExpandoObject message2, IMiFuService<TMessage>[] receivers);

        /// <summary>
        /// Sends a message synchronously e.g. to query another service. This method ends in Async per the MS
        /// design guidelines for asyncronous methods.
        /// </summary>
        /// <remarks>NOTE: This requires the message to be self-describing with respect to destination.</remarks>
        /// <param name="message"></param>
        /// <param name="message2">The inbound message converted into an ExpandoObject.</param>
        /// <param name="receivers"></param>
        /// <returns></returns>
        Task<Result2<TMessage>> SendAsync(TMessage message, ExpandoObject message2, IMiFuService<TMessage>[] receivers);
    }
}
