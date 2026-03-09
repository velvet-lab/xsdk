namespace xSdk.Data;

internal class InternalDatabaseSetup : DatabaseSetup
{
    #region Only for Initialization of Repository in Factory needed

    internal IDatabaseSetup Setup { get; set; } = null!;

    internal Type DatabaseType { get; set; } = null!;

    internal Type ConnectionBuilderType { get; set; } = null!;

    internal string Name { get; set; } = string.Empty;

    #endregion
}
