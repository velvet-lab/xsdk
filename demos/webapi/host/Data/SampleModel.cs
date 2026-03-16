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

using System.ComponentModel;
using xSdk.Data;

namespace xSdk.Demos.Data;

[Description("A sample model")]
public sealed class SampleModel : Model, IModel<GuidStringPK, string>
{
    public SampleModel()
    {
        this.PrimaryKey = new GuidStringPK();
    }

    [Description("The id of the sample model")]
    public new string Id
    {
        get => PrimaryKey.GetValue<string>();
        set => PrimaryKey.SetValue(value);
    }

    [Description("The name of the sample model")]
    public string Name { get; set; }
}
