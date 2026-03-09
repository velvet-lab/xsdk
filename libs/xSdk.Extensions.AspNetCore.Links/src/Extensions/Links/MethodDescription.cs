using System.Reflection;

namespace xSdk.Extensions.Links;

internal sealed class MethodDescription
{
    public MethodInfo Action { get; internal set; }

    public Type ControllerType { get; internal set; }

    public HttpMethod HttpMethod { get; internal set; }

    public string? MethodName { get; internal set; }

    public string? PolicyName { get; internal set; }

    public string? RouteTemplate { get; internal set; }

    public string[] AuthRoles { get; internal set; } = Array.Empty<string>();

    public string? AuthPolicy { get; internal set; }

    internal bool ShouldAuthorize
    {
        get
        {
            if (AuthRoles.Any() || !string.IsNullOrEmpty(AuthPolicy))
            {
                return true;
            }
            return false;
        }
    }

    public override string ToString()
    {
        return MethodName ?? this.GetType().Name;
    }
}
