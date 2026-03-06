using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace xSdk.Extensions.Mermaid
{
    internal class StateDiagramMarks
    {
        internal const string Diagram = "stateDiagram";
        internal const string Transition = "-->";
        internal const string Value = ":";
        internal const string Inital = "[*]";
        internal const string Final = Inital;
        internal const string State = "state";
        internal const string OpenBracket = "{";
        internal const string CloseBracket = "}";
        internal const string Comment = "%%";
        internal const string Choice = "<<choice>>";
        internal const string Fork = "<<fork>>";
        internal const string Join = "<<join>>";
    }
}
