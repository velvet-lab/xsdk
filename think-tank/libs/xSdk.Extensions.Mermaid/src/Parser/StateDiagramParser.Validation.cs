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
        private void ValidateParseResults()
        {
            // Revalidate Diagram
            var states = CollectStates();
            foreach (var state in states)
            {
                var composition = FilterCompositionsByName(Compositions, state.Name);
                state.IsComposition = composition != null;

                var fork = Forks.SingleOrDefault(x => string.Compare(x.Name, state.Name, true) == 0);
                state.IsFork = fork != null;

                var join = Joins.SingleOrDefault(x => string.Compare(x.Name, state.Name, true) == 0);
                state.IsJoin = join != null;

                var choice = Choices.SingleOrDefault(x => string.Compare(x.Name, state.Name, true) == 0);
                state.IsChoice = choice != null;

                var description = Descriptions.SingleOrDefault(x => string.Compare(x.Name, state.Name, true) == 0);
                if (description != null && !string.IsNullOrEmpty(description.Text))
                {
                    state.Text = description.Text;
                }
            }
        }

        private IEnumerable<State> CollectStates()
        {
            var states = Transitions.Select(x => x.Source);
            states = states.Concat(Transitions.Select(x => x.Target));
            states = states.Concat(CollectStatesFromComposition(Compositions));

            return states;
        }

        private IEnumerable<State> CollectStatesFromComposition(IEnumerable<Composition> compositions)
        {
            IEnumerable<State> result = new List<State>();
            foreach (var composition in compositions)
            {
                result = result.Concat(composition.Transitions.Select(y => y.Source));
                result = result.Concat(composition.Transitions.Select(y => y.Target));

                var subStates = CollectStatesFromComposition(composition.Compositions);
                if (subStates != null)
                {
                    result = result.Concat(subStates);
                }
            }

            return result;
        }
    }
}
