using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NLog;
using xSdk.Extensions.Variable.Attributes;
using xSdk.Shared;

namespace xSdk.Extensions.Variable;

public abstract class Setup : ISetup
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    private readonly ICollection<ValidationResult> _validationResults = new Collection<ValidationResult>();
    private VariableService _variableService;
    private VariableService _slimVariableService;

    public bool IsSlimMode => _slimVariableService != null;

    public ICollection<ValidationResult> Results => _validationResults;

    public void Validate() => Validate(true);

    public void Validate(bool throwIfFails)
    {
        bool isValid = true;

        var currentType = this.GetType();

        _logger.Info("Validate Setup '{0}'", currentType.FullName);
        _logger.Trace("Validate Annotations for Setup '{0}'", currentType.FullName);
        if (!this.ValidateAnnotations(out ICollection<ValidationResult> annotationResults))
        {
            isValid = annotationResults.ValidateResults();
        }

        if (isValid)
        {
            _logger.Trace("Validate Setup '{0}'", currentType.FullName);
            ValidateSetup();

            isValid = Results.ValidateResults();
        }

        if (!isValid && throwIfFails)
        {
            throw new SdkException(string.Format("Setup '{0}' not valid.", currentType.FullName));
        }
    }

    internal void InitializeInternal(VariableService service)
    {
        _variableService = service;
        CreateVariables();
        Initialize();
    }

    protected virtual void ValidateSetup() { }

    protected virtual void Initialize() { }

    protected TValue ReadValue<TValue>(string name) => ReadValue<TValue>(name, false);

    protected TValue ReadValue<TValue>(string name, bool shouldThrowIfNotFound)
    {
        var variable = GetVariableService().LoadVariable(name);
        if (variable != null)
        {
            return GetVariableService().ReadVariableValue<TValue>(name, shouldThrowIfNotFound);
        }

        return default(TValue);
    }

    protected void SetValue<TValue>(string name, TValue value)
    {
        var variable = GetVariableService().LoadVariable(name);
        if (variable != null)
        {
            GetVariableService().SetVariable(name, value);
        }
    }

    private void CreateVariables()
    {
        // Remarks: Dont activate Logging, because its produces a StackOverFlow
        var createMethod = typeof(Variable).GetMethod("Create", BindingFlags.Static | BindingFlags.Public, new Type[] { typeof(string) });
        if (createMethod != null)
        {
            // Read all properties of the Implementation
            var setupType = GetType();

            var mainPrefix = setupType
                .Name.Replace("implementation", "", StringComparison.InvariantCultureIgnoreCase)
                .Replace("setup", "", StringComparison.InvariantCultureIgnoreCase);

            var prefixAttribute = setupType.GetAttribute<VariablePrefixAttribute>();
            var noprefixAttribute = setupType.GetAttribute<VariableNoPrefixAttribute>();
            if (prefixAttribute != null)
            {
                mainPrefix = prefixAttribute.Prefix;
            }

            if (noprefixAttribute != null)
            {
                mainPrefix = null;
            }

            foreach (var property in setupType.GetProperties())
            {
                var attr = property.GetAttribute<VariableAttribute>();
                if (attr != null)
                {
                    object defaultValue = attr.DefaultValue;
                    object currentValue = null;
                    try
                    {
                        currentValue = property.GetValue(this);
                    }
                    catch
                    {
                        // Nothing to tell
                    }

                    if (IsDefaultValueGreater(property.PropertyType, currentValue, defaultValue))
                    {
                        currentValue = null;
                    }

                    // Set Default Value
                    if (currentValue == null && defaultValue != null)
                    {
                        if (!TypeConverter.IsEmpty(defaultValue, property.PropertyType))
                        {
                            defaultValue = TypeConverter.ConvertTo(defaultValue, property.PropertyType);
                        }
                    }

                    // Create Variable
                    var genericCreateMethod = createMethod.MakeGenericMethod(property.PropertyType);
                    if (genericCreateMethod != null)
                    {
                        var name = property.Name;
                        if (!string.IsNullOrEmpty(attr.Name))
                        {
                            name = attr.Name;
                        }

                        var variable = genericCreateMethod.Invoke(null, new object[] { name.ToLower() }) as Variable;
                        if (variable != null)
                        {
                            if (!string.IsNullOrEmpty(mainPrefix))
                            {
                                variable.SetPrefix(mainPrefix);
                            }

                            if (!string.IsNullOrEmpty(attr.Prefix))
                            {
                                variable.SetPrefix(attr.Prefix);
                            }

                            if (defaultValue != null)
                            {
                                variable.SetDefaultValue(defaultValue);
                            }

                            if (!string.IsNullOrEmpty(attr.HelpText))
                            {
                                variable.SetHelpText(attr.HelpText);
                            }

                            if (!string.IsNullOrEmpty(attr.Template))
                            {
                                variable.SetTemplate(attr.Template);
                            }

                            if (attr.Protect)
                            {
                                variable.Protect();
                            }

                            if (attr.Hidden)
                            {
                                variable.Hide();
                            }

                            if (attr.NoPrefix)
                            {
                                variable.DisablePrefix();
                            }

                            variable.SetAttribute(attr);
                            variable.SetTelemetryResourceValueDelegate(() => property.GetValue(this));

                            GetVariableService().AddVariableFromSetupInitialize(variable);

                            // Try to read Environment for the correct Value
                            var readMethod = typeof(VariableService).GetMethod(
                                "ReadVariableValueInternal",
                                BindingFlags.NonPublic | BindingFlags.Instance,
                                new Type[] { typeof(string), typeof(bool), typeof(bool) }
                            );
                            if (readMethod != null)
                            {
                                var genericReadMethod = readMethod.MakeGenericMethod(property.PropertyType);
                                if (genericReadMethod != null)
                                {
                                    var environmentValue = genericReadMethod.Invoke(GetVariableService(), new object[] { name, false, false });
                                    if (environmentValue != null && !TypeConverter.IsEmpty(environmentValue, property.PropertyType))
                                    {
                                        if (!variable.IsProtected)
                                        {
                                            // Set the readed Value
                                            property.SetValue(this, environmentValue);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool IsDefaultValueGreater(Type type, object currentValue, object defaultValue)
    {
        if (currentValue != null && defaultValue != null && type.IsEnum)
        {
            var noneEnumValue = Enum.GetName(type, 0b_0000_0000);
            if (Enum.TryParse(type, currentValue.ToString(), out object currentEnum))
            {
                var currentEnumValue = Enum.GetName(type, currentEnum);
                if (currentEnumValue == noneEnumValue)
                {
                    if (Enum.TryParse(type, defaultValue.ToString(), out object defaultEnum))
                    {
                        var defaultEnumValue = Enum.GetName(type, defaultEnum);
                        if (defaultEnumValue != noneEnumValue)
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    private VariableService GetVariableService()
    {
        if (_variableService == null)
        {
#nullable disable
            if (_slimVariableService == null)
            {
                var config = new ConfigurationBuilder().AddInMemoryCollection().Build();
                var services = new ServiceCollection();
                services.AddSlimVariableServices(config, true);

                var provider = services.BuildServiceProvider();
                _slimVariableService = provider.GetRequiredService<IVariableService>() as VariableService;
                CreateVariables();
                Initialize();
            }
            return _slimVariableService;
#nullable restore
        }
        else
        {
            return _variableService;
        }
    }
}
