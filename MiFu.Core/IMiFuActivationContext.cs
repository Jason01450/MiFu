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

namespace MiFu.Core
{
    public interface IMiFuActivationContext<TMessage>
    {
        ///// <summary>
        ///// The incoming message parsed into an Expando.
        ///// </summary>
        ///// <returns></returns>
        //ExpandoObject GetParsedMessage();

        /// <summary>
        /// The incoming message.
        /// </summary>
        TMessage Message { get; }

        /// <summary>
        /// Access to the (abstracted) message transport.
        /// </summary>
        IMiFuTransport<TMessage> Transport { get; }
    }
}
