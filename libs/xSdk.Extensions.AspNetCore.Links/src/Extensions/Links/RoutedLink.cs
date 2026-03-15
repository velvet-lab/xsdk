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

using Microsoft.AspNetCore.Http;
using xSdk.Data;

namespace xSdk.Extensions.Links;

public abstract class RoutedLink(string name, string methodName)
{
    public string Name => name;

    public string MethodName => methodName;

    internal MethodDescription? Description { get; set; }

    internal HttpContext? Context { get; set; }

    internal IModel? Model { get; set; }

    internal abstract IHateoasItem? Build();
}

public class RoutedLink<TModel>(string name, string methodName, Func<TModel, object> values) : RoutedLink(name, methodName)
    where TModel : IModel
{
    public Func<TModel, object> Values => values;

    internal TModel? ConcreteModel
    {
        get
        {
            if (Model is TModel concreteModel)
            {
                return concreteModel;
            }
            return default;
        }
    }

    internal override IHateoasItem? Build()
    {
        var builder = new RoutedLinkBuilder();
        return builder.Build<TModel>(this);
    }
}
