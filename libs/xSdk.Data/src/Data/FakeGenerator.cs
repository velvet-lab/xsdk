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
using xSdk.Tools;

namespace xSdk.Data;

public static class FakeGenerator
{
    private const string DefaultContext = "default";
    private static Dictionary<string, object>? _fakers;
    private static readonly int _globalCount = Random.Shared.Next(1, 10);

    public static TEntity Generate<TFake, TEntity>()
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateAsync<TFake, TEntity>(DefaultContext, false).GetAwaiter().GetResult();

    public static TEntity Generate<TFake, TEntity>(bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateAsync<TFake, TEntity>(DefaultContext, strictMode).GetAwaiter().GetResult();

    public static TEntity Generate<TFake, TEntity>(string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateAsync<TFake, TEntity>(context, false).GetAwaiter().GetResult();

    public static TEntity Generate<TFake, TEntity>(string context, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateAsync<TFake, TEntity>(context, strictMode).GetAwaiter().GetResult();

    public static TEntity Generate<TEntity>(Type fake)
        where TEntity : class
        => GenerateAsync<TEntity>(fake, DefaultContext, false).GetAwaiter().GetResult();

    public static TEntity Generate<TEntity>(Type fake, bool strictMode)
        where TEntity : class
        => GenerateAsync<TEntity>(fake, DefaultContext, strictMode).GetAwaiter().GetResult();

    public static TEntity Generate<TEntity>(Type fake, string context)
        where TEntity : class
        => GenerateAsync<TEntity>(fake, context, false).GetAwaiter().GetResult();

    public static TEntity Generate<TEntity>(Type fake, string context, bool strictMode)
        where TEntity : class
        => GenerateAsync<TEntity>(fake, context, strictMode).GetAwaiter().GetResult();

    public static Task<TEntity> GenerateAsync<TFake, TEntity>()
        where TFake : Fakes<TEntity>, new()
        where TEntity : class =>
        GenerateListAsync<TFake, TEntity>(1, DefaultContext, false, false)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TFake, TEntity>(bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class =>
        GenerateListAsync<TFake, TEntity>(1, DefaultContext, false, strictMode)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TFake, TEntity>(string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(1, context, false, false)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TFake, TEntity>(string context, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(1, context, false, strictMode)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TEntity>(Type fake)
        where TEntity : class =>
        GenerateListAsync<TEntity>(fake, 1, DefaultContext, false, false)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TEntity>(Type fake, bool strictMode)
        where TEntity : class =>
        GenerateListAsync<TEntity>(fake, 1, DefaultContext, false, strictMode)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TEntity>(Type fake, string context)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, 1, context, false, false)
            .ContinueWith(task => task.Result.Single());

    public static Task<TEntity> GenerateAsync<TEntity>(Type fake, string context, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, 1, context, false, strictMode)
            .ContinueWith(task => task.Result.Single());

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>()
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(string context, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(string context, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, string context)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, string context, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, string context, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count, string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, context, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count, string context, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, context, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TFake, TEntity>(int count, string context, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, context, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, repeatableData, strictMode).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count, string context)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, context, false, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count, string context, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, context, repeatableData, false).GetAwaiter().GetResult();

    public static IEnumerable<TEntity> GenerateList<TEntity>(Type fake, int count, string context, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, context, repeatableData, strictMode).GetAwaiter().GetResult();

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>()
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, DefaultContext, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(string context, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(string context, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(_globalCount, context, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, DefaultContext, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, string context)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, string context, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, string context, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, _globalCount, context, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TFake, TEntity>(count, DefaultContext, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count, string context)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TEntity>(typeof(TFake), count, context, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count, string context, bool repeatableData)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TEntity>(typeof(TFake), count, context, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TFake, TEntity>(int count, string context, bool repeatableData, bool strictMode)
        where TFake : Fakes<TEntity>, new()
        where TEntity : class
        => GenerateListAsync<TEntity>(typeof(TFake), count, context, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, int count)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, int count, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, int count, bool repeatableData, bool strictMode)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, DefaultContext, repeatableData, strictMode);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, int count, string context)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, context, false, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fake, int count, string context, bool repeatableData)
        where TEntity : class
        => GenerateListAsync<TEntity>(fake, count, context, repeatableData, false);

    public static Task<IEnumerable<TEntity>> GenerateListAsync<TEntity>(Type fakeType, int count, string context, bool repeatableData, bool strictMode)
        where TEntity : class
    {
        _fakers ??= [];

        string? baseContext = fakeType.FullName;
        if (string.IsNullOrEmpty(context))
        {
            context = DefaultContext;
        }

        string completeContext = $"{baseContext}_{context}";
        if (!_fakers.ContainsKey(completeContext))
        {
            object? faker = InitFaker<TEntity>(fakeType, context, repeatableData, strictMode);
            if (faker != null)
            {

                _fakers.AddOrNew(completeContext, faker);
            }
        }

        if (_fakers[completeContext] is Faker<TEntity> bogus)
        {
            return Task.FromResult<IEnumerable<TEntity>>(bogus.Generate(count));
        }

        return Task.FromResult<IEnumerable<TEntity>>([]);
    }

    private static object? InitFaker<TEntity>(Type fakeType, string context, bool repeatableData, bool strictMode)
        where TEntity : class
    {
        if (Activator.CreateInstance(fakeType) is Fakes<TEntity> fake)
        {
            var bogus = new Faker<TEntity>();

            if (repeatableData)
            {
                Randomizer.Seed = new Random(85416985); // DevSkim: ignore DS148264
            }

            if (strictMode)
            {
                bogus.StrictMode(strictMode);
            }

            fake.BuildInternal(bogus, context);

            return bogus;
        }

        return null;
    }
}
