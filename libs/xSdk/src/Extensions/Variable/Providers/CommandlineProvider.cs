using xSdk.Extensions.Commands;

namespace xSdk.Extensions.Variable.Providers
{
    internal sealed class CommandlineProvider : VariableProviderBase
    {
        protected override bool ExistsVariable(IVariable variable)
        {
            if (variable != null)
            {
                return CommandlineParser.Parse().ContainsPattern(Cast(variable).KeyForCommandline);
            }

            return false;
        }

        protected override object ReadVariable(IVariable variable)
        {
            if (CommandlineParser.Parse().ContainsPattern(Cast(variable).KeyForCommandline))
            {
                var args = CommandlineParser.Parse().Arguments;
                if (args.Length > 1)
                {
                    var index = args.ToList().IndexOf(Cast(variable).KeyForCommandline);

                    // Retrieve current value
                    string value = args[index];

                    // Try to get next value
                    var nextValue = args[index];
                    if (args.Length > index + 1)
                    {
                        nextValue = args[index + 1];
                    }

                    if (nextValue.StartsWith("--"))
                    {
                        // Current Value is a switch value
                        object switchValue = null;
                        if (string.Compare(value, Cast(variable).KeyForCommandline, true) == 0)
                        {
                            switchValue = true;
                        }

                        if (switchValue != null)
                        {
                            return switchValue;
                        }
                    }
                    else
                    {
                        return nextValue;
                    }

                    //if (!value.StartsWith("--"))
                    //{
                    //    return value;
                    //}
                    //else
                    //{
                    //    // It seems its a Switch Value
                    //    object switchValue = null;
                    //    value = args[index];
                    //    if (string.Compare(value, variable.KeyForCommandline, true) == 0)
                    //    {
                    //        switchValue = true;
                    //    }

                    //    if (switchValue != null)
                    //    {
                    //        return switchValue;
                    //    }
                    //}
                }
            }
            return null;
        }
    }
}
