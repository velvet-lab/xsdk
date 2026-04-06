using System.Diagnostics;
using xSdk.Extensions.Commands;
using xSdk.Extensions.IO;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;

namespace xSdk.Extensions.Options;

public sealed partial class EnvironmentOptions
{
    [Variable(
        name: DefaultCommandSettings.Definitions.Stage.Name,
        template: DefaultCommandSettings.Definitions.Stage.Template,
        helpText: DefaultCommandSettings.Definitions.Stage.HelpText,
        defaultValue: DefaultCommandSettings.Definitions.Stage.DefaultValue,
        resourceNames: ["{{app.prefix}}.environment.stage", "deployment.environment"]
    )]
    public Stage Stage
    {
        get => ReadValue<Stage>(DefaultCommandSettings.Definitions.Stage.Name);
        set => SetValue(DefaultCommandSettings.Definitions.Stage.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.Demo.Name,
        template: DefaultCommandSettings.Definitions.Demo.Template,
        helpText: DefaultCommandSettings.Definitions.Demo.HelpText,
        resourceNames: ["{{app.prefix}}.environment.demo"]
    )]
    public bool IsDemo
    {
        get => ReadValue<bool>(DefaultCommandSettings.Definitions.Demo.Name);
        set => SetValue(DefaultCommandSettings.Definitions.Demo.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.ContentRoot.Name,
        template: DefaultCommandSettings.Definitions.ContentRoot.Template,
        helpText: DefaultCommandSettings.Definitions.ContentRoot.HelpText
    )]
    public string ContentRoot
    {
        get => ReadValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
        set => SetValue(DefaultCommandSettings.Definitions.ContentRoot.Name, value);
    }

    [Variable(
        name: DefaultCommandSettings.Definitions.LogLevel.Name,
        template: DefaultCommandSettings.Definitions.LogLevel.Template,
        helpText: DefaultCommandSettings.Definitions.LogLevel.HelpText,
        defaultValue: DefaultCommandSettings.Definitions.LogLevel.DefaultValue
    )]
    public string LogLevel
    {
        get => ReadValue<string>(DefaultCommandSettings.Definitions.LogLevel.Name);
        set => SetValue(DefaultCommandSettings.Definitions.LogLevel.Name, value);
    }

    private static string DetermineContentRoot()
    {
        var contentRoot = ReadCommandlineValue<string>(DefaultCommandSettings.Definitions.ContentRoot.Name);
        if (string.IsNullOrEmpty(contentRoot))
        {
            contentRoot = Environment.CurrentDirectory;

            if (Debugger.IsAttached)
            {
                contentRoot = FileSystemHelper.SearchGitRoot(contentRoot);
            }
        }

        return contentRoot;
    }

    private static TValue? ReadCommandlineValue<TValue>(string pattern)
    {
        var result = string.Empty;

        var parser = CommandlineParser.Parse();
        if (parser.ContainsPattern(pattern))
        {
            result = parser.ReadPattern(pattern);
        }

        if (!string.IsNullOrEmpty(result))
        {
            return TypeConverter.ConvertTo<TValue>(result);
        }

        return default(TValue);
    }

    
}
