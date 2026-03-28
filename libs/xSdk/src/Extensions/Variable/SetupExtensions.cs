/*
 * Copyright 2026 Roland Breitschaft
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using Microsoft.Extensions.Logging;
using xSdk.Hosting;

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
                    _logger.LogWarning(
                        "Implementation is not valid for Member '{0}'. (Reason: {1})",
                        result.MemberNames.Aggregate((a, b) => a + ", " + b),
                        result.ErrorMessage
                    );
                }
                else
                {
                    _logger.LogWarning("Implementation is not valid. (Reason: {0})", result.ErrorMessage);
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
