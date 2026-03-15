using xSdk.Extensions.Variable;

namespace xSdk.Plugins.Documentation;

public interface IDocumentationSetup : ISetup
{
    bool Enabled { get; set; }

    string DocumentPattern { get; set; }
}
