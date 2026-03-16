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

using Microsoft.Extensions.DependencyInjection;
using xSdk.Extensions.Mermaid.Lexer;
using xSdk.Extensions.Mermaid.Parser;

namespace xSdk.Extensions.Mermaid
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMermaid(this IServiceCollection services)
        {
            services.AddSingleton<ILexerService, LexerService>().AddSingleton<IParserService, ParserService>().AddSingleton<IMermaidService, MermaidService>();

            services.AddSingleton<StateDiagramParser>();

            return services;
        }
    }
}
