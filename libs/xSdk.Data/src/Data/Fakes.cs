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

using Bogus;

namespace xSdk.Data;

public abstract class Fakes<TEntity>
    where TEntity : class
{
    protected string? Context { get; private set; }

    public virtual TEntity GetExamples()
    {
        var result = FakeGenerator.Generate<TEntity>(this.GetType(), false);

        return result;
    }

    internal void BuildInternal(Faker<TEntity> builder, string context)
    {
        Context = context;

        Build(builder);
    }

    protected abstract void Build(Faker<TEntity> builder);

    protected bool IsContext(string context)
    {
        if (Context == context)
            return true;

        return false;
    }


}
