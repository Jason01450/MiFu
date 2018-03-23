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

namespace MiFu.Core
{
    /// <summary>
    /// The primary interface that all MiFu microservices must implement in order to be activated
    /// (receive messages) from the MiFu framework.
    /// </summary>
    /// <typeparam name="TMessage"></typeparam>
    public interface IMiFuService<TMessage>
    {
        object Activate(IMiFuActivationContext<TMessage> context);
    }
}
