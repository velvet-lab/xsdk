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

//using Microsoft.Agents.ObjectModel;
//using Microsoft.Extensions.AI;
//using Microsoft.Extensions.DependencyInjection;

//namespace xSdk.Extensions.AI.obsolete;

//public class AIDefinition
//{
//    public string? Name => Metadata?.Name;

//    public string? Description => Metadata?.Description;

//    public string? Instructions => Metadata?.Instructions?.ToTemplateString();

//    public string? Model => Metadata?.Model?.ExtensionData?.ReadValue("name");

//    public GptComponentMetadata? Metadata { get; internal set; }

//    public string? FilePath { get; internal set; }

//    public IEnumerable<AIFunction> LoadTools(IServiceProvider provider)
//    {
//        // Nur Tools injizieren, die im YAML deklariert sind
//        IEnumerable<string?>? toolNames = Metadata?.Tools.Select(x => x.GetType().GetProperty("Name")?.GetValue(x) as string);
//        if (toolNames is not null && toolNames.Any())
//        {
//            foreach (string? toolName in toolNames.Where(toolName => !string.IsNullOrEmpty(toolName)))
//            {
//                AIFunction? tool = provider.GetKeyedService<AIFunction>(toolName);
//                if (tool is not null)
//                {
//                    yield return tool;
//                }
//            }
//        }
//    }
//}
