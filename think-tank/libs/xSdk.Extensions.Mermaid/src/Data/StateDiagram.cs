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
    public sealed class StateDiagram : Diagram
    {
        public StateDiagram()
            : base(DiagramType.StateDiagram)
        {
            States = new List<State>();
        }

        //internal ICollection<Transition> Transitions { get; }

        //internal ICollection<Composition> Compositions { get; }

        //internal ICollection<Choice> Choices { get; }

        //internal ICollection<Fork> Forks { get; }

        //internal ICollection<Join> Joins { get; }

        //internal ICollection<Description> Descriptions { get; }

        public IEnumerable<State> States { get; internal set; }
    }
}
