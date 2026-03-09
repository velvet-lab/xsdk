namespace xSdk.Extensions.Plugin;

public interface IPluginDescription
{
    string? Name { get; }

    Version? Version { get; }

    string? Description { get; }

    string? ProductVersion { get; }

    string? Tag { get; }

    List<string> Tags { get; }
}
