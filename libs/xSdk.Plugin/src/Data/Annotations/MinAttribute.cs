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

namespace xSdk.Data.Annotations;

/// <summary>
/// Min primaryKey attribute.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class MinAttribute : DataAnnotationAttribute
{
    /// <summary>
    /// Initializes a new instance of the MinAttribute class.
    /// </summary>
    /// <param name="value">The minimum primaryKey.</param>
    public MinAttribute(object value)
        : base(value) { }

    public override bool IsValid(object value)
    {
        if (IsIntValue())
        {
            var configured = GetIntValue();
            var current = (int)Value;

            if (current < configured)
                return false;
        }
        else if (IsDoubleValue())
        {
            var configured = GetDoubleValue();
            var current = (double)Value;

            if (current < configured)
                return false;
        }
        else if (IsTimeSpanValue())
        {
            var configured = GetTimeSpanValue();
            var current = (TimeSpan)Value;

            if (current < configured)
                return false;
        }

        return true;
    }
}
