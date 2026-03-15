using Bogus;

namespace xSdk.Data;

public abstract class Fakes<TEntity>
    where TEntity : class
{
    protected string? Context { get; private set; }

    public virtual TEntity GetExamples()
    {
        var result = FakeGenerator.Generate<TEntity>(this.GetType(), false);

        return result;
    }

    internal void BuildInternal(Faker<TEntity> builder, string context)
    {
        Context = context;

        Build(builder);
    }

    protected abstract void Build(Faker<TEntity> builder);

    protected bool IsContext(string context)
    {
        if (Context == context)
            return true;

        return false;
    }


}
