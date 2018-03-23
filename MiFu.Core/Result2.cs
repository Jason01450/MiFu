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
    /// Utility class used to indicate both [1] the success/failure of a method and
    /// [2] the result of that method. In the success case the .Error property is null.
    /// In the failure case, the .Value property is the default, and .Error contains
    /// an error message as a string.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Result2<T>
    {
        public string Error { get; private set; }

        public T Value { get; private set; }

        private Result2(T value, string error = null)
        {
            Value = value;
            Error = (error ?? string.Empty).Trim();
        }

        public bool IsOK { get { return string.IsNullOrEmpty(Error); } }

        public static Result2<T> Fail(string error)
        {
            return new Result2<T>(default(T), error);
        }

        public static Result2<T> Fail(T value, string error)
        {
            return new Result2<T>(value, error);
        }

        public static Result2<T> OK(T value)
        {
            return new Result2<T>(value);
        }

        public override string ToString()
        {
            var status = IsOK ? "OK" : "ERR";
            return $"{status}:" + (IsOK ? Value.ToString() : Error);
        }
    }
}
