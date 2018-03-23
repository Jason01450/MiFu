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

using System.Dynamic;

namespace MiFu.Core
{
    public interface IMiFuRegistry<TMessage> where TMessage : class
    {
        void Register(IMiFuService<TMessage> service, string[] filters);

        IMiFuService<TMessage>[] FindMatches(ExpandoObject message);

        IMiFuService<TMessage>[] FindMatches(string message);
    }
}
