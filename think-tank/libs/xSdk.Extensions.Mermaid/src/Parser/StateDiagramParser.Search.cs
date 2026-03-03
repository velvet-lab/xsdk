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
