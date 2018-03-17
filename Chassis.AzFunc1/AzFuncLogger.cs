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

using Microsoft.Azure.WebJobs.Host;
using System;
using System.Diagnostics;

namespace MiFu.Chassis.AzFunc1
{

    /// <summary>
    /// Wrapper class for TraceWriter instance. This is motivated by the lack of an interface for TraceWriter.
    /// MS uses an abstract class, which makes mocking it out challenging. Wish this was unecessary.
    /// </summary>
    public class AzFuncLogger : IAzFuncLogger
    {
        public AzFuncLogger(TraceWriter writer)
        {
            Writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        public TraceLevel Level { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public void Error(string message, Exception ex = null, string source = null)
        {
            Writer.Error(message, ex, source);
        }

        public void Flush() { Writer.Flush(); }

        public void Info(string message, string source = null) { Writer.Info(message, source); }

        public void Trace(TraceEvent traceEvent) { Writer.Trace(traceEvent); }

        public void Verbose(string message, string source = null) { Writer.Verbose(message, source); }

        public void Warning(string message, string source = null) { Writer.Warning(message, source); }

        public TraceWriter Writer { get; private set; }
    }
}
