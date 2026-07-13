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

using xSdk.Plugins.AI;

namespace xSdk.Demos;

public static class OllamaConfiguration
{
    public static void Default(AIOptions options)
    {
        // options.Model = "phi4-mini";
        //options.Model = "Qwen/Qwen2.5-VL-7B-Instruct-AWQ";
        //options.EmbeddingModel = "qwen3-embedding:0.6b";
        options.Path = Environment.CurrentDirectory;
    }
}
