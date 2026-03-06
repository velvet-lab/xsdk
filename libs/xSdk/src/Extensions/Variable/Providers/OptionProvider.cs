using xSdk.Hosting;
using Microsoft.Extensions.Configuration;

namespace xSdk.Extensions.Variable.Providers
{
    internal class OptionProvider(IConfiguration configuration) : VariableProviderBase
    {
        protected override bool ExistsVariable(IVariable variable)
        {
            var result = ReadVariable(variable);

            return result != null;
        }

        protected override object? ReadVariable(IVariable variable)
        {
            if (configuration != null && variable != null)
            {
                var mainSection = configuration.GetSection(SlimHost.Instance.AppPrefix.ToLower());
                if (mainSection != null)
                {
                    var sectionName = variable.Name;
                    if (!string.IsNullOrEmpty(variable.Prefix))
                        sectionName = variable.Prefix;

                    try
                    {
                        var result = ReadValue(mainSection, variable);
                        if (result == null)
                        {
                            var section = mainSection.GetSection(NormalizeName(variable.Prefix, sectionName, true));
                            if (section != null)
                            {
                                result = ReadValue(section, variable);
                                return result;
                            }
                        }
                        else
                        {
                            return result;
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
            return default;
        }

        private string NormalizeName(string? prefix, string name, bool isSection)
        {
            if (!isSection)
            {
                if (!string.IsNullOrEmpty(prefix) && name.StartsWith(prefix))
                {
                    name = name.Substring(prefix.Length);
                }
            }

            if (name.IndexOf("-") > -1)
                name = name.Replace("-", "");

            return name.Trim();
        }

        private object? ReadValue(IConfigurationSection section, IVariable variable) =>
            section.GetValue(variable.ValueType, NormalizeName(variable.Prefix, variable.Name, false));
    }
}
