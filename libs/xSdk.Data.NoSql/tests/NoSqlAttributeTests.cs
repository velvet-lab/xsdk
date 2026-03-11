namespace xSdk.Data;

public class NoSqlAttributeTests
{
    // NoSqlIndexAttribute tests

    [Fact]
    public void NoSqlIndexAttribute_DefaultCtor_HasAttributeUsage()
    {
        var attr = new NoSqlIndexAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_WithUnique_IsApplicable()
    {
        var attr = new NoSqlIndexAttribute(unique: true);

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_WithExpressionAndUnique_IsApplicable()
    {
        var attr = new NoSqlIndexAttribute("$.Name", true);

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIndexAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlIndexAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }

    // NoSqlIgnoreAttribute tests

    [Fact]
    public void NoSqlIgnoreAttribute_DefaultCtor_IsNotNull()
    {
        var attr = new NoSqlIgnoreAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlIgnoreAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlIgnoreAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }

    // NoSqlFieldAttribute tests

    [Fact]
    public void NoSqlFieldAttribute_DefaultCtor_IsNotNull()
    {
        var attr = new NoSqlFieldAttribute();

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlFieldAttribute_WithName_IsNotNull()
    {
        var attr = new NoSqlFieldAttribute("myField");

        Assert.NotNull(attr);
    }

    [Fact]
    public void NoSqlFieldAttribute_AttributeTarget_IsPropertyOrField()
    {
        var usage = typeof(NoSqlFieldAttribute)
            .GetCustomAttributes(typeof(AttributeUsageAttribute), inherit: false)
            .Cast<AttributeUsageAttribute>()
            .Single();

        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Property));
        Assert.True(usage.ValidOn.HasFlag(AttributeTargets.Field));
    }
}
