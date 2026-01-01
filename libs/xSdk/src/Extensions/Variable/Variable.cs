using xSdk.Extensions.Variable.Attributes;
using xSdk.Hosting;
using xSdk.Shared;

namespace xSdk.Extensions.Variable
{
    public class Variable : IVariable
    {
        private string _template;
        private string applicationPrefix;

        protected Variable(string name, Type valueType)
        {
            Name = name;
            if (valueType == null)
                throw new ArgumentNullException("valueType");

            ValueType = valueType;

            applicationPrefix = SlimHost.Instance.AppPrefix;
        }

        public string Name { get; private set; }

        public Type ValueType { get; private set; }

        public string Prefix { get; internal set; }

        public bool NoPrefix { get; internal set; }

        public string Template
        {
            get => CreateTemplate(_template);
            internal set => _template = value;
        }

        public string HelpText { get; internal set; }

        public bool IsProtected { get; internal set; }

        public bool IsHidden { get; internal set; }

        internal Func<object> TelemetryResourceValue { get; set; }

        internal VariableAttribute Attribute { get; set; }

        protected internal string KeyForSystem => CreateKey(false, true).Trim().ToUpperInvariant();

        protected internal string KeyForFile => $"{KeyForSystem}{Globals.Constants.PREFIX_SEPERATOR}file".Trim().ToUpperInvariant();

        protected internal string KeyForCommandline => CreateKey(true, false).Trim().ToLowerInvariant();

        public static Variable Create(string name, Type type) => Create(name, type, default);

        public static Variable Create(string name, Type type, Action<Variable>? configure)
        {
            var result = new Variable(name, type);

            configure?.Invoke(result);

            return result;
        }

        public static Variable<TType> Create<TType>(string name) => Create<TType>(name, default);

        public static Variable<TType> Create<TType>(string name, Action<Variable<TType>>? configure)
        {
            var result = new Variable<TType>(name);

            configure?.Invoke(result);

            return result;
        }

        internal virtual Variable SetDefaultValue(object defaultValue) => this;

        public override int GetHashCode() => ObjectHelper.CreateHashCode(this.ToString());

        public override string ToString() => CreateKey(false, false);

        public override bool Equals(object obj) =>
            ObjectHelper.Equals<Variable>(this, obj, (source, dest) => string.CompareOrdinal(source.ToString(), dest.ToString()) == 0);

        internal string CreateKey(bool forCommandline, bool withApplicationPrefix)
        {
            var variableName = Name;
            var appPrefix = applicationPrefix;
            var prefix = Prefix;

            if (NoPrefix)
            {
                appPrefix = string.Empty;
                prefix = string.Empty;
            }

            var result = $"{prefix}{Globals.Constants.PREFIX_SEPERATOR}";
            if (!string.IsNullOrEmpty(result) && result.StartsWith(Globals.Constants.PREFIX_SEPERATOR))
            {
                result = result.Substring(Globals.Constants.PREFIX_SEPERATOR.Length);
            }

            if (forCommandline)
            {
                result = "";
            }

            var key = variableName;
            if (!string.IsNullOrEmpty(prefix))
            {
                if (key.StartsWith(prefix))
                {
                    key = key.Substring(prefix.Length)?.Trim();
                    key = RemoveUnwantedCharsOnFirstPosition(key);
                }
            }

            result = $"{result}{key}";
            if (forCommandline)
            {
                if (!string.IsNullOrEmpty(prefix))
                {
                    result = $"{prefix}{Globals.Constants.VARIABLE_SEPERATOR}{result}";
                }
                result = $"--{result.Replace(Globals.Constants.VARIABLE_SEPERATOR, "-")}";
            }
            else
            {
                if (withApplicationPrefix && !string.IsNullOrEmpty(appPrefix))
                {
                    result = $"{appPrefix}{Globals.Constants.PREFIX_SEPERATOR}{result}";
                }
                result = result.Replace("-", Globals.Constants.VARIABLE_SEPERATOR);
            }

            return result;
        }

        private string CreateTemplate(string value)
        {
            var templateValue = value;
            if (!string.IsNullOrEmpty(templateValue) && templateValue.IndexOf("<") > -1)
            {
                templateValue = templateValue.Substring(templateValue.IndexOf("<") + 1);
                templateValue = templateValue.Substring(0, templateValue.IndexOf(">"));
            }
            else
            {
                templateValue = string.Empty;
            }

            if (!string.IsNullOrEmpty(templateValue))
            {
                templateValue = $" <{templateValue?.Trim()}>";
            }

            return $"{KeyForCommandline}{templateValue}".Trim();
        }

        private static string RemoveUnwantedCharsOnFirstPosition(string name)
        {
            var tokens = new List<string> { "-", "." };
            tokens.ForEach(x =>
            {
                if (name.StartsWith(x))
                {
                    name = name.Substring(1);
                }
            });
            return name;
        }
    }

    public sealed partial class Variable<TType> : Variable
    {
        internal Variable(string name)
            : base(name, typeof(TType)) { }

        public TType DefaultValue { get; private set; }

        internal override Variable SetDefaultValue(object defaultValue)
        {
            try
            {
                if (defaultValue.GetType() != this.ValueType)
                {
                    throw new SdkException("Value Type is different from Variable Type");
                }

                DefaultValue = (TType)defaultValue;
            }
            catch
            {
                // Ignore Exception
            }

            return this;
        }
    }
}
