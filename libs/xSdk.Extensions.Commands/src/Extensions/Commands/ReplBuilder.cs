using xSdk.Extensions.Variable;
using xSdk.Hosting;

namespace xSdk.Extensions.Commands
{
    internal class ReplBuilder : IReplBuilder
    {
        public string Prompt { get; set; }

        public Func<string> PromptFactory { get; set; }

        public ReplBuilder()
        {
            var envSetup = SlimHost.Instance.VariableSystem.GetSetup<EnvironmentSetup>();
            Prompt = ">";
            PromptFactory = () => string.Format("{0} {1} ", envSetup.AppName, Prompt);
        }
    }
}
