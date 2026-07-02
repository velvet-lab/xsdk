using System.CommandLine;
using System.Reflection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Commands.Attributes;
using xSdk.Extensions.Logging;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal static class OptionManager
{
    private static ILogger Logger => field ??= LogManager.CreateLogger(typeof(OptionManager));

    internal static Option[] Build(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return Build(properties);
    }

    internal static Option[] Build(PropertyInfo[] properties)
    {
        var result = new List<Option>();
        foreach (PropertyInfo property in properties)
        {
            var option = Build(property);
            if (option is not null)
            {
                result.Add(option);
            }

        }
        return result.ToArray();
    }

    internal static Option? Build(PropertyInfo property)
    {
        var attribute = property.GetAttribute<CommandOptionAttribute>();
        if (attribute is null)
        {
            return default;
        }

        var genericOptionType = typeof(Option<>).MakeGenericType(property.PropertyType);
        var constructor = genericOptionType.GetConstructor(new Type[] { typeof(string), typeof(string[]) });
        if (constructor is not null)
        {
            var name = Normalizer.NormalizeOptionName(attribute.Name.ToLowerInvariant());
            var aliases = Normalizer.NormalizeOptionAliases(attribute.Aliases);


            var optionObject = constructor.Invoke(new object[] { name, aliases }) as Option;
            if (optionObject is not null)
            {

                optionObject.Description = property.Description();
                optionObject.Required = property.IsRequired();
                return optionObject;
            }
        }

        return default;
    }

    internal static void Inject(ICommandHandler handler, ParseResult parseResult)
    {
        var properties = handler.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var property in properties)
        {
            var optionAttribute = property.GetAttribute<CommandOptionAttribute>();
            if (optionAttribute is not null)
            {
                Inject(property, optionAttribute, parseResult, handler);
            }
        }
    }

    private static void Inject(PropertyInfo property, CommandOptionAttribute attribute, ParseResult parseResult, ICommandHandler handler)
    {
        var getValueMethod = parseResult.GetType().GetMethod("GetValue", new Type[] { typeof(string) });
        if (getValueMethod is not null)
        {
            var genericGetValueMethod = getValueMethod.MakeGenericMethod(property.PropertyType);
            if (genericGetValueMethod is not null)
            {
                var currentValue = genericGetValueMethod.Invoke(parseResult, new object[] { Normalizer.NormalizeOptionName(attribute.Name) });
                if (currentValue is not null)
                {
                    property.SetValue(handler, currentValue);
                }
            }
        }
    }
}
