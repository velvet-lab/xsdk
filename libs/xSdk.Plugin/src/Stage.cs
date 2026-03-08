namespace xSdk;

[Flags]
public enum Stage
{
    /*
     * "Development", "Integration", "Production
     */
    None = 0b_0000_0000,
    Development = 0b_0000_0001,
    Integration = 0b_0000_0010,
    PreProduction = 0b_0000_0100,
    Production = 0b_0000_1000,
    All = Development | Integration | PreProduction | Production,
}
