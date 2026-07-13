namespace xSdk.Extensions.Builder;

public static class IBuilderExtensions
{
    extension(IBuilder builder)
    {
        public TBuilder AsBuilder<TBuilder>()
            where TBuilder : class, IBuilder
        {
            if (builder is not TBuilder)
            {
                throw new InvalidOperationException($"The builder is not of type {typeof(TBuilder).FullName}.");
            }

            return (TBuilder)builder;
        }
    }
}
