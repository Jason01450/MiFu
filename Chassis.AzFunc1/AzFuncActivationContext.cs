﻿// Copyright 2018 Jason W. Weber
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
using MiFu.Core;

namespace MiFu.Chassis.AzFunc1
{
    public class AzFuncActivationContext : IMiFuActivationContext<string>
    {
        public string Message { get; private set; }

        public IMiFuTransport<string> Transport { get; private set; }

        public AzFuncActivationContext(string message, IMiFuTransport<string> transport)
        {
            Message = message ?? throw new ArgumentNullException(nameof(message));
            Transport = transport ?? throw new ArgumentNullException(nameof(transport));
        }
    }
}
