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

//using Microsoft.Extensions.DependencyInjection;
//using xSdk.Extensions.Options;
//using xSdk.Extensions.Plugin;
//using xSdk.Plugins.AI;
//using xSdk.Tools;

//namespace xSdk.Extensions.AI;

//public abstract class AIPluginBuilder : PluginBuilder, IAIPluginBuilder
//{
//    private readonly Dictionary<Type, IAILayerBuilder> _aiLayerBuilders = [];

//    public abstract void Initialize();

//    protected IAILayerBuilder<TClient> CreateAILayer<TClient>(Func<TClient> value)
//        where TClient : class
//    {
//        Type key = typeof(TClient);
//        if (_aiLayerBuilders.TryGetValue(key, out IAILayerBuilder? existingBuilder) && existingBuilder is IAILayerBuilder<TClient> typedBuilder)
//        {
//            return typedBuilder;
//        }
//        else
//        {
//            var aiLayer = new AILayerBuilder<TClient>(value);
//            _aiLayerBuilders.AddOrNew(key, aiLayer);
//            return aiLayer;
//        }
//    }

//    internal string[] GetRegisteredAgentKeys()
//    {
//        return [.. _aiLayerBuilders.Values.SelectMany(x => x.Definitions.Select(y => y.Name))];
//    }

//    internal void InitializeLayers(IServiceCollection services, PluginOptions? pluginOptions, EnvironmentOptions? environmentOptions)
//    {
//        foreach (IAILayerBuilder builder in _aiLayerBuilders.Values)
//        {
//            builder.Build(services, pluginOptions, environmentOptions);
//        }
//    }
//}
