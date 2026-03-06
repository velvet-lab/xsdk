using xSdk.Data;

namespace xSdk.Extensions.Mermaid.Data
{
    public class Diagram : Model
    {
        protected Diagram(DiagramType diagramType)
        {
            DiagramType = diagramType;
        }

        public string Title { get; internal set; }

        public string Version { get; internal set; }

        public DiagramType DiagramType { get; }
    }
}
