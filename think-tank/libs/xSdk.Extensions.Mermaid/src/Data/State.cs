/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections.Generic;
using xSdk.Data;

namespace xSdk.Extensions.Mermaid.Data
{
    public sealed class State : Model, ISupportName
    {
        public State()
        {
            Childs = new List<State>();
        }

        public ICollection<State> Childs { get; set; }

        public string Condition { get; set; }

        public string Name { get; internal set; }

        public string Text { get; internal set; }

        public bool IsFork { get; internal set; }

        public bool IsJoin { get; internal set; }

        public bool IsChoice { get; internal set; }

        public string Comment { get; internal set; }

        internal bool IsInitial { get; set; }

        internal bool IsFinal { get; set; }

        internal bool IsComposition { get; set; }

        public override string ToString() => Name;
    }
}
