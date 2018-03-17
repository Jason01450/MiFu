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
using System.Diagnostics;
using Microsoft.Azure.WebJobs.Host;

namespace MiFu.Chassis.AzFunc1
{
    public interface IAzFuncLogger
    {
        //
        // Summary:
        //     Gets or sets the System.Diagnostics.TraceLevel filter used to filter trace events.
        //     Only trace events with a System.Diagnostics.TraceLevel less than or equal to
        //     this level will be traced. Level filtering will be done externally by the Microsoft.Azure.WebJobs.JobHost,
        //     so it shouldn't be done by this class.
        TraceLevel Level { get; set; }

        //
        // Summary:
        //     Writes a System.Diagnostics.TraceLevel.Error level trace event.
        //
        // Parameters:
        //   message:
        //     The trace message.
        //
        //   ex:
        //     The optional System.Exception for the error.
        //
        //   source:
        //     The source of the message.
        void Error(string message, Exception ex = null, string source = null);
        
        //
        // Summary:
        //     Flush any buffered trace events.
        void Flush();

        //
        // Summary:
        //     Writes a System.Diagnostics.TraceLevel.Info level trace event.
        //
        // Parameters:
        //   message:
        //     The trace message.
        //
        //   source:
        //     The source of the message.
        void Info(string message, string source = null);

        //
        // Summary:
        //     Writes a trace event.
        //
        // Parameters:
        //   traceEvent:
        //     The Microsoft.Azure.WebJobs.Host.TraceEvent to trace.
        void Trace(TraceEvent traceEvent);

        //
        // Summary:
        //     Writes a System.Diagnostics.TraceLevel.Verbose level trace event.
        //
        // Parameters:
        //   message:
        //     The trace message.
        //
        //   source:
        //     The source of the message.
        void Verbose(string message, string source = null);

        //
        // Summary:
        //     Writes a System.Diagnostics.TraceLevel.Warning level trace event.
        //
        // Parameters:
        //   message:
        //     The trace message.
        //
        //   source:
        //     The source of the message.
        void Warning(string message, string source = null);
    }
}
