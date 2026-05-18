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

using xSdk.Data;

namespace xSdk.Demos.Data;

internal class SampleDatabase
{
    private List<SampleModel> _database = [];
    private static SampleDatabase? _singleton;

    public static List<SampleModel> Load()
    {
        _singleton ??= new SampleDatabase
            {
                _database = [.. FakeGenerator.GenerateList<SampleModelExamples, SampleModel>(10)]
        };

        return _singleton._database;
    }

    public static List<SampleModel> Save(SampleModel model)
    {
        if (_singleton is null)
        {
            _singleton = new SampleDatabase();
            _singleton._database.Add(model);
        }

        return _singleton._database;
    }
}
