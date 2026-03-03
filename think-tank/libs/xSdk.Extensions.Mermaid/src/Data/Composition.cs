using System.Collections.Generic;

namespace xSdk.Extensions.Mermaid.Data
{
    internal sealed class Composition : ISupportName, ISupportComments
    {
        public Composition()
        {
            Transitions = new List<Transition>();
            Compositions = new List<Composition>();
        }

        public Composition Parent { get; internal set; }

        public string Name { get; internal set; }

        public string Comment { get; set; }

        public ICollection<Transition> Transitions { get; }

        public ICollection<Composition> Compositions { get; }

        public override string ToString() => Name;
    }
}
