namespace xSdk.Extensions.Mermaid.Data
{
    internal sealed class Join : ISupportName, ISupportComments
    {
        public string Name { get; internal set; }

        public string Comment { get; set; }
    }
}
