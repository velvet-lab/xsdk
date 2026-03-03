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
