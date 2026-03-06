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
