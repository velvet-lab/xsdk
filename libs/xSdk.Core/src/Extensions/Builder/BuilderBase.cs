using FluentValidation;
using Microsoft.Extensions.Logging;
using xSdk.Extensions.Logging;

namespace xSdk.Extensions.Builder;

public abstract class BuilderBase : IBuilder
{
    private ILogger Logger { get => field ??= LogManager.CreateLogger<BuilderBase>(); }

    public virtual void ConfigureBuilder()
    {
    }

    protected internal IServiceProvider SlimServices
    {
        get => field ?? throw new InvalidOperationException("SlimServices has not been initialized.");
        set;
    }

    protected bool IsValid<TBuilder, TValidator>()
        where TValidator : AbstractValidator<TBuilder>, new()
        where TBuilder : BuilderBase
    {
        TBuilder instance = (TBuilder) this;
        var validator = new TValidator();        
        
        var result = validator.Validate(instance);
        if(!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Logger.LogError(error.ErrorMessage);
            }
            return false;
        }

        return true;
    }    
}
