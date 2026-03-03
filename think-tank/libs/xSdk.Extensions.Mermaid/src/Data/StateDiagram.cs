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
