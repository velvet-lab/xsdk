using FluentValidation;
using xSdk.Extensions.Variable;

namespace xSdk.Extensions.Options;

internal class DefaultOptionsValidator<TOptions> : AbstractValidator<TOptions>
    where TOptions : class, IVariableSetup
{
    public DefaultOptionsValidator()
    {
        
    }
}
