namespace xSdk.Extensions.Commands
{
    public interface IReplBuilder
    {
        string Prompt { get; set; }

        Func<string> PromptFactory { get; set; }
    }
}
