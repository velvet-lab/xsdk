using xSdk.Shared;
using Microsoft.Extensions.Configuration;
using NLog;
using System.Collections;
using System.Collections.Concurrent;

namespace xSdk.Extensions.Variable
{
    internal partial class VariableService : IVariableService
    {
        private readonly IConfiguration? _config;

        private Logger logger = LogManager.GetCurrentClassLogger();

        public VariableService(IConfiguration? config)
        {
            this._config = config;

            InitProviders();
        }

        public Dictionary<string, object> ToDictionary()
        {
            var result = new Dictionary<string, object>();

            foreach (var variable in this.Variables)
            {
                try
                {
                    if (TryReadVariableValue<object>(variable.Name, out object value))
                    {
                        result.AddOrNew(variable.Name, value);
                    }
                    else
                    {
                        logger.Warn("Variable Value '{0}' not found", variable.Name);
                    }
                }
                catch
                {
                    // Nothing to tell
                }
            }

            return result;
        }

        internal void AddEnvironmentVariables()
        {
            var items = Environment.GetEnvironmentVariables();

            // GetPrimaryKey Items to Dictionary
            var dic = new ConcurrentDictionary<string, object>();
            foreach (DictionaryEntry item in items)
            {
                dic.AddOrNew(item.Key.ToString(), item.Value);
            }

            // Execute in Parallel
            Parallel.ForEach(
                dic,
                item =>
                {
                    var value = item.Value?.ToString();
                    var valueType = TypeConverter.GetValueType(value);

                    if (valueType != null)
                    {
                        var name = item.Key.ToString();
                        var variable = this.LoadVariableInternal(name);
                        if (variable == null)
                        {
                            variable = Variable.Create(name, valueType).Protect().DisablePrefix().Hide();

                            NewVariable(variable);
                        }
                    }
                }
            );
        }
    }
}
