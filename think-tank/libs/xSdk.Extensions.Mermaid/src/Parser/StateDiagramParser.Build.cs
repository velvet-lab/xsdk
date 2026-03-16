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
        private IEnumerable<State> BuildObjectModel()
        {
            var topTransitions = Transitions.Where(x => x.Source.IsInitial && !x.Target.IsChoice);

            var initialState = new State { IsInitial = true };

            BuildStates(topTransitions, initialState);

            return initialState.Childs;
        }

        private void BuildStates(IEnumerable<Transition> transitions, State parent)
        {
            foreach (var transition in transitions)
            {
                var state = BuildState(transition, parent);
                if (state != null)
                {
                    if (transition.Source.IsChoice)
                    {
                        state.Condition = transition.Value;
                        state.IsChoice = true;
                    }
                    else if (transition.Source.IsFork)
                    {
                        state.IsFork = true;
                    }
                    else if (transition.Target.IsJoin)
                    {
                        state.IsJoin = true;
                    }

                    parent.Childs.Add(state);

                    var nextTransistions = SearchSourceTransitions(transition.Target);
                    BuildStates(nextTransistions, state);
                }
            }
        }

        private State BuildState(Transition transition, State parent)
        {
            var source = transition.Source;
            var target = transition.Target;

            State result = null;

            if (!target.IsFinal)
            {
                if (target.IsChoice || target.IsFork || target.IsJoin)
                {
                    var transitions = SearchSourceTransitions(target);
                    BuildStates(transitions, parent);
                }
                else if (target.IsComposition)
                {
                    var compositionTransitions = SearchCompositionTransitions(target);
                    BuildStates(compositionTransitions, parent);
                }
                else
                {
                    result = target;
                }
            }

            return AddDescription(result);
        }

        private State AddDescription(State state)
        {
            if (state != null)
            {
                var description = Descriptions.FirstOrDefault(x => string.Compare(state.Name, x.Name, true) == 0);
                if (description != null)
                {
                    state.Comment = description.Comment;
                }
            }

            return state;
        }
    }
}
