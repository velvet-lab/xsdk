namespace xSdk.Data;

public class GuidStringPKTests
{
    [Fact]
    public void GuidStringPK_DefaultConstructor_GeneratesNewGuidString()
    {
        var pk = new GuidStringPK();

        var value = pk.GetValue<string>();
        Assert.NotNull(value);
        Assert.True(Guid.TryParse(value, out _));
    }

    [Fact]
    public void GuidStringPK_GuidConstructor_StoresAsString()
    {
        var guid = Guid.NewGuid();

        var pk = new GuidStringPK(guid);

        Assert.Equal(guid.ToString(), pk.GetValue<string>());
    }

    [Fact]
    public void GuidStringPK_StringConstructor_ParsesAndStores()
    {
        var guid = Guid.NewGuid();

        var pk = new GuidStringPK(guid.ToString());

        Assert.Equal(guid.ToString(), pk.GetValue<string>());
    }

    [Fact]
    public void GuidStringPK_GetValueAsGuid_ReturnsGuid()
    {
        var guid = Guid.NewGuid();
        var pk = new GuidStringPK(guid);

        var result = pk.GetValue<Guid>();

        Assert.Equal(guid, result);
    }

    [Fact]
    public void GuidStringPK_EqualityOperator_SameValues_ReturnsTrue()
    {
        var guid = Guid.NewGuid();
        var pk1 = new GuidStringPK(guid);
        var pk2 = new GuidStringPK(guid);

        Assert.True(pk1 == pk2);
    }

    [Fact]
    public void GuidStringPK_EqualityOperator_DifferentValues_ReturnsFalse()
    {
        var pk1 = new GuidStringPK(Guid.NewGuid());
        var pk2 = new GuidStringPK(Guid.NewGuid());

        Assert.False(pk1 == pk2);
    }

    [Fact]
    public void GuidStringPK_InequalityOperator_DifferentValues_ReturnsTrue()
    {
        var pk1 = new GuidStringPK(Guid.NewGuid());
        var pk2 = new GuidStringPK(Guid.NewGuid());

        Assert.True(pk1 != pk2);
    }

    [Fact]
    public void GuidStringPK_ToString_ReturnsStringRepresentation()
    {
        var guid = Guid.NewGuid();
        var pk = new GuidStringPK(guid);

        Assert.Equal(guid.ToString(), pk.ToString());
    }
}
