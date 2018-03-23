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
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MiFu.Core
{
    public class MiFuRegistry<TMessage> : IMiFuRegistry<TMessage> where TMessage : class
    {
        private class RoutingEntry
        {
            public KeyValuePair<string, string>[] Rules { get; private set; }

            public IMiFuService<TMessage> Service { get; private set; }

            public RoutingEntry(KeyValuePair<string, string>[] rules, IMiFuService<TMessage> service)
            {

                Rules = rules;
                Service = service;
            }
        }

        public static readonly char Delimiter = ':';

        public static KeyValuePair<string, string> ParseRule(string rule)
        {
            if (string.IsNullOrWhiteSpace(rule))
                throw new ArgumentOutOfRangeException(
                    "Must include one or more non-whitespace characters!", nameof(rule));

            var test = new Regex(RegEx);
            var matches = test.Matches(rule);
            if (matches.Count != 1)
                throw new ArgumentOutOfRangeException(
                    "Must include non-whitespace key and value portions seperated by a colon!", nameof(rule));

            var parts = rule.Split(Delimiter);
            return new KeyValuePair<string, string>(parts[0].ToLower(), parts[1]);
        }

        public static KeyValuePair<string, string>[] ParseRules(string[] rules)
        {
            if (rules == null)
                throw new ArgumentNullException(nameof(rules));

            List<KeyValuePair<string, string>> hold = new List<KeyValuePair<string, string>>();
            foreach (var rule in rules)
                hold.Add(ParseRule(rule));
            KeyValuePair<string, string>[] theArray = hold.ToArray();
            return theArray;
        }

        public static readonly string RegEx = @"^([a-zA-Z](\-?[a-zA-Z0-9]+)*):(([a-zA-Z](\-?[a-zA-Z0-9]+)*)|(([a-zA-Z](\-?[a-zA-Z0-9]+)*)\-?\*)|\*)$";

        public void Register(IMiFuService<TMessage> service, string[] filters)
        {
            if (service == null)
                throw new ArgumentNullException(nameof(service));
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            List<KeyValuePair<string, string>> rules = new List<KeyValuePair<string, string>>();
            foreach (var filter in filters)
                rules.Add(ParseRule(filter));
            KeyValuePair<string, string>[] theArray = rules.ToArray();

            try
            {
                safety.AcquireWriterLock(1000);
                try
                {
                    serviceList.Add(new RoutingEntry(theArray, service));
                }
                finally
                {
                    safety.ReleaseWriterLock();
                }
            }
            catch (ApplicationException ex)
            {
                // TODO: Log the timeout!
                Console.WriteLine($"Timed out! {ex.Message}");
            }
        }

        private static readonly IMiFuService<TMessage>[] NoMatches = new IMiFuService<TMessage>[0];

        public IMiFuService<TMessage>[] FindMatches(ExpandoObject message)
        {
            var matches = NoMatches;
            try
            {
                safety.AcquireReaderLock(1000); // milliseconds
                try
                {
                    matches = serviceList.Where(x => IsMatch(x, message)).Select(x => x.Service).ToArray();
                }
                finally
                {
                    safety.ReleaseReaderLock();
                }
            }
            catch (ApplicationException ex)
            {
                // TODO: Log the timeout!
                Console.WriteLine($"Timed out! {ex.Message}");
            }
            return matches;
        }

        public IMiFuService<TMessage>[] FindMatches(string message)
        {
            ExpandoObject cast = JsonConvert.DeserializeObject<ExpandoObject>(message, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            });
            return FindMatches(cast);
        }

        private static bool IsMatch(RoutingEntry entry, ExpandoObject message)
        {
            var lookup = ((IDictionary<string, object>)message);
            foreach (var rule in entry.Rules)
            {
                var fieldName = rule.Key;
                if (!lookup.ContainsKey(fieldName))
                    return false;

                if (rule.Value == "*")
                    continue;

                var fieldValue = (string)lookup[fieldName];
                if (rule.Value != fieldValue)
                    return false;
            }
            return true;
        }

        private ReaderWriterLock safety = new ReaderWriterLock();

        private List<RoutingEntry> serviceList = new List<RoutingEntry>();
    }
}
