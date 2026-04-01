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

using xSdk.Demos.Controllers.v3;
using xSdk.Demos.Data;
using xSdk.Extensions.Links;
using xSdk.Extensions.Plugin;
using xSdk.Plugins.Links;

namespace xSdk.Demos.Builders;

internal class LinksPluginBuilder : PluginBuilderBase, ILinksPluginBuilder
{
    public void ConfigureLinks(LinksOptions options)
    {
        options
            .AddPolicy<SampleModel>(policy =>
            {
                policy
                    .RequireRoutedLink("all", nameof(SampleController.GetSamplesHateOasAsync))
                    .RequireRoutedLink("new", nameof(SampleController.SaveSampleHateOasAsync))
                    .RequireRoutedLink("self", nameof(SampleController.GetSampleHateOasAsync), x => new { Id = x.Id })
                    .RequireRoutedLink("edit", nameof(SampleController.UpdateSampleHateOasAsync), x => new { Id = x.Id })
                    .RequireRoutedLink("delete", nameof(SampleController.DeleteSampleHateOasAsync), x => new { Id = x.Id });
            });
    }
}
