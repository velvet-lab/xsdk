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

namespace xSdk.Extensions.Mermaid.Data
{
    internal sealed class Transition : ISupportComments
    {
        public Transition()
        {
            Childs = new List<Transition>();
        }

        public ICollection<Transition> Childs { get; }

        public State Source { get; internal set; }

        public State Target { get; internal set; }

        public string Value { get; internal set; }

        public string Comment { get; set; }

        public override string ToString() => $"{Source.Name} {StateDiagramMarks.Transition} {Target.Name}";
    }
}
