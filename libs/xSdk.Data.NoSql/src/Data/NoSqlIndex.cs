namespace xSdk.Data
{
    internal sealed class NoSqlIndex
    {
        public string Field { get; set; }
        public string Expression { get; set; }
        public bool IsUnique { get; set; }
    }
}
