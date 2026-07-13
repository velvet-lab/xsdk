using System.CommandLine;

namespace xSdk.Extensions.Commands;

public static class ReplConsoleBuilderExtension
{
    extension(ReplConsoleBuilder builder)
    {
        public ReplConsoleBuilder WithBanner(Action factory)
        {
            builder.CreateBannerAction = factory;
            return builder;
        }

        public ReplConsoleBuilder WithLastWill(Action factory)
        {
            builder.CreateLastWillAction = factory;
            return builder;
        }

        public ReplConsoleBuilder WithUserPrompt(Func<string> factory)
        {
            builder.CreateUserPromptAction = factory;
            return builder;
        }

        public ReplConsoleBuilder WithCustomHelp(Action<IList<Command>> factory)
        {
            builder.CreateHelpAction = factory;
            return builder;
        }
    }
}
