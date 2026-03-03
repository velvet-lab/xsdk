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
