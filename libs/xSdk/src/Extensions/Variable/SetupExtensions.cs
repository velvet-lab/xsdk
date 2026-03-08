using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using NLog;

namespace xSdk.Extensions.Variable;

public static class SetupExtensions
{
    private static readonly ILogger _logger = LogManager.GetCurrentClassLogger();

    public static bool ValidateAnnotations(this ISetup setup, out ICollection<ValidationResult> results) => setup.ValidateAnnotations(out results, null);

    public static bool ValidateAnnotations(this ISetup setup, out ICollection<ValidationResult> results, string[] allowedEmptyProperties)
    {
        var ctx = new ValidationContext(setup);
        results = new List<ValidationResult>();

        var tmpResults = new List<ValidationResult>();
        if (!Validator.TryValidateObject(setup, ctx, tmpResults, true))
        {
            if (allowedEmptyProperties != null)
            {
                foreach (var error in tmpResults)
                {
                    if (error.MemberNames != null)
                    {
                        foreach (var memberName in error.MemberNames)
                        {
                            if (!allowedEmptyProperties.Contains(memberName))
                            {
                                results.Add(error);
                            }
                        }
                    }
                }
            }
        }

        return results.Count == 0;
    }

    public static bool ValidateResults(this IEnumerable<ValidationResult> results)
    {
        if (results != null && results.Any())
        {
            foreach (var result in results)
            {
                if (result.MemberNames != null && result.MemberNames.Any())
                {
                    _logger.Warn(
                        "Implementation is not valid for Member '{0}'. (Reason: {1})",
                        result.MemberNames.Aggregate((a, b) => a + ", " + b),
                        result.ErrorMessage
                    );
                }
                else
                {
                    _logger.Warn("Implementation is not valid. (Reason: {0})", result.ErrorMessage);
                }

                return false;
            }
        }
        return true;
    }

    public static void ValidateMember<TSetup>(this TSetup setup, Expression<Func<TSetup, bool>> validator)
        where TSetup : ISetup => setup.ValidateMember(validator, null);

    public static void ValidateMember<TSetup>(this TSetup setup, Expression<Func<TSetup, bool>> validator, string errorMessage)
        where TSetup : ISetup => setup.ValidateMember(validator, errorMessage, Array.Empty<string>());

    public static void ValidateMember<TSetup>(this TSetup setup, Expression<Func<TSetup, bool>> validator, string errorMessage, params string[] memberNames)
        where TSetup : ISetup
    {
        var hasError = false;

        try
        {
            var validatorFunc = validator.Compile();
            hasError = validatorFunc(setup);
        }
        catch
        {
            hasError = true;
        }

        if (hasError)
        {
            if (!setup.Results.Any(x => x.ErrorMessage == errorMessage))
            {
                setup.Results.Add(new ValidationResult(errorMessage, memberNames));
            }
        }
    }
}
