using System.CommandLine;
using System.Reflection;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Commands.Attributes;
using xSdk.Extensions.Logging;
using xSdk.Tools;

namespace xSdk.Extensions.Commands;

internal static class ArgumentManager
{
    private static ILogger Logger => field ??= LogManager.CreateLogger(typeof(ArgumentManager));

    internal static Argument[] Build(Type type)
    {
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
        return Build(properties);
    }

    internal static Argument[] Build(PropertyInfo[] properties)
    {
        var result = new List<Argument>();
        foreach (PropertyInfo property in properties)
        {
            var argument = Build(property);
            if (argument is not null)
            {
                result.Add(argument);
            }

        }
        return result.ToArray();
    }

    internal static Argument? Build(PropertyInfo property)
    {
        var attribute = property.GetAttribute<CommandArgumentAttribute>();
        if (attribute is null)
        {
            return default;
        }

        var genericArgumentType = typeof(Argument<>).MakeGenericType(property.PropertyType);
        var constructor = genericArgumentType.GetConstructor(new Type[] { typeof(string) });
        if (constructor is not null)
        {
            var name = attribute.Name.ToLowerInvariant();

            var argumentObject = constructor.Invoke(new object[] { name }) as Argument;
            if (argumentObject is not null)
            {
                argumentObject.Description = property.Description();
                return argumentObject;
            }
        }

        return default;
    }

    internal static void Inject(ICommandHandler handler, ParseResult parseResult)
    {
        var properties = handler.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
        foreach (var property in properties)
        {
            var argumentAttribute = property.GetAttribute<CommandArgumentAttribute>();
            if (argumentAttribute is not null)
            {
                Inject(property, argumentAttribute, parseResult, handler);
            }
        }
    }

    private static void Inject(PropertyInfo property, CommandArgumentAttribute attribute, ParseResult parseResult, ICommandHandler handler)
    {
        try
        {
            var getValueMethod = parseResult.GetType().GetMethod("GetValue", new Type[] { typeof(string) });
            if (getValueMethod is not null)
            {
                var genericGetValueMethod = getValueMethod.MakeGenericMethod(property.PropertyType);
                if (genericGetValueMethod is not null)
                {
                    var currentValue = genericGetValueMethod.Invoke(parseResult, new object[] { attribute.Name.ToLowerInvariant() });
                    if (currentValue is not null)
                    {
                        property.SetValue(handler, currentValue);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Logger.LogWarning(ex, "Failed to inject argument '{attributeName}' into property '{propertyName}' of handler '{handlerName}'.", attribute.Name, property.Name, handler.GetType().Name);
        }
    }
}
