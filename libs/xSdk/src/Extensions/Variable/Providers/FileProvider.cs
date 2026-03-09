using xSdk.Shared;

namespace xSdk.Extensions.Variable.Providers;

internal sealed class FileProvider : VariableProviderBase
{
    protected override bool ExistsVariable(IVariable variable)
    {
        if (variable != null)
        {
            var fileName = EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForFile);
            if (!string.IsNullOrEmpty(fileName))
                return File.Exists(fileName);
        }

        return false;
    }

    protected override object? ReadVariable(IVariable variable)
    {
        string? result = default;

        var fileName = EnvironmentTools.ReadEnvironmentVariable(Cast(variable).KeyForFile);
        if (!string.IsNullOrEmpty(fileName))
        {
            if (File.Exists(fileName))
            {
                result = File.ReadAllText(fileName);
            }
        }

        return result;
    }
}
