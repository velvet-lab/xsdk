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
using System.Linq;
using xSdk.Extensions.Mermaid.Data;

namespace xSdk.Extensions.Mermaid.Parser
{
    internal sealed partial class StateDiagramParser
    {
        internal static Composition FilterCompositionsByName(ICollection<Composition> compositions, string name)
        {
            foreach (var composition in compositions)
            {
                if (string.Compare(composition.Name, name, true) == 0)
                {
                    return composition;
                }
                else
                {
                    return FilterCompositionsByName(composition.Compositions, name);
                }
            }

            return null;
        }

        private IEnumerable<Transition> SearchSourceTransitions(State state)
        {
            return Transitions.Where(x => string.Compare(x.Source.Name, state.Name, true) == 0);
        }

        private IEnumerable<Transition> SearchCompositionTransitions(State state)
        {
            var composition = FilterCompositionsByName(Compositions, state.Name);

            if (composition != null)
            {
                return composition.Transitions;
            }

            return new List<Transition>();
        }
    }
}
