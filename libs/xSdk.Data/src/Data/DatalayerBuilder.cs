using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace xSdk.Data
{
    public sealed class DatalayerBuilder : IDatalayerBuilder
    {
        private IServiceCollection _services;
        private List<string> _logicalNames;

        internal DatalayerBuilder(IServiceCollection services)
        {
            _services = services ?? throw new ArgumentNullException(nameof(services));
            _logicalNames = new List<string>();
        }

        public IDatalayerBuilder ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(string name)
            where TDatabase : class, IDatabase
            where TDatabaseSetup : IDatabaseSetup, new()
            where TConnectionStringBuilder : class, IConnectionBuilder => ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(name, null);

        public IDatalayerBuilder ConfigureDatabase<TDatabase, TDatabaseSetup, TConnectionStringBuilder>(string name, Action<TDatabaseSetup> factory)
            where TDatabase : class, IDatabase
            where TDatabaseSetup : IDatabaseSetup, new()
            where TConnectionStringBuilder : class, IConnectionBuilder
        {
            ValidateLogicalNames(name, ref _logicalNames);

            var setup = new TDatabaseSetup();
            factory?.Invoke(setup);

            setup.Validate();

            _services.AddSingleton<TConnectionStringBuilder>();
            _services.AddScoped<TDatabase>();
            _services.AddSingleton(
                new InternalDatabaseSetup
                {
                    Setup = setup,
                    Name = name,
                    ConnectionBuilderType = typeof(TConnectionStringBuilder),
                    DatabaseType = typeof(TDatabase),
                }
            );

            return this;
        }

        public IDatalayerBuilder ConfigureRepository<TImplementation>(IEnumerable<string> dataProviders)
            where TImplementation : class, IRepository
        {
            _services.TryAddScoped<TImplementation>(provider =>
            {
                var instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
                InitializeRepository(provider, instance, dataProviders);

                return instance;
            });

            return this;
        }

        public IDatalayerBuilder ConfigureRepository<TInterface, TImplementation>(IEnumerable<string> dataProviders)
            where TInterface : class
            where TImplementation : class, IRepository, TInterface
        {
            _services.TryAddScoped<TInterface>(provider =>
            {
                var instance = ActivatorUtilities.CreateInstance<TImplementation>(provider);
                InitializeRepository(provider, instance, dataProviders);

                return instance;
            });

            return this;
        }

        internal void InitializeRepository(IServiceProvider provider, IRepository repository, IEnumerable<string> dataProviders)
        {
            var setups = provider.GetServices<InternalDatabaseSetup>();

            ValidateLogicalNames(setups.Select(x => x.Name));

            if (!setups.Any())
                throw new SdkException("No Datalayer Setups found");

            IEnumerable<InternalDatabaseSetup> databaseSetups;
            if (setups.Count() > 1)
            {
                if (!dataProviders.Any())
                    throw new SdkException(
                        $"No Data Provider Name specified. Add the Data Provider Name to Repository Mappings, to specify the Data Provider, which the Repository '{repository.GetType().FullName}' should used"
                    );

                databaseSetups = setups.Where(x => dataProviders.Any(y => string.Compare(x.Name, y, true) == 0));
                if (!databaseSetups.Any())
                    throw new SdkException($"No Datalayer Setups found for Mappings '{dataProviders}'");
            }
            else
                databaseSetups = setups;

            ((Repository)repository).InternalSetups = databaseSetups;
        }

        private void ValidateLogicalNames(IEnumerable<string> names)
        {
            var cleanedNamesCount = names.Distinct().Count();
            if (cleanedNamesCount != names.Count())
                throw new SdkException($"Database Logical Names are not unique. Please choose a another Name to register the Database");
        }

        private void ValidateLogicalNames(string name, ref List<string> names)
        {
            if (names.Any(x => string.Compare(x, name, true) == 0))
                throw new SdkException($"Database with Name '{name}' is already registered. Please choose a another Name to register the Database");

            names.Add(name);
        }

        //private void ConfigureFramework(IDatabaseSetup setup)
        //{
        //    if (setup.Dialect != Dialect.LiteDb && setup.Dialect != Dialect.None)
        //    {
        //        if (setup.Dialect == Dialect.DB2)
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.DB2);
        //        else if (setup.Dialect == Dialect.MySQL)
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.MySQL);
        //        else if (setup.Dialect == Dialect.Oracle)
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.Oracle);
        //        else if (setup.Dialect == Dialect.PostgreSQL)
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
        //        else if (setup.Dialect == Dialect.SQLite)
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLite);
        //        else
        //            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.SQLServer);

        //        SqlMapper.AddTypeHandler<Guid>(new GuidTypeHandler());
        //    }
        //    else if (setup.Dialect == Dialect.LiteDb)
        //    { }
        //    else
        //        throw new AminOOException("No Dialect choosed for Repository");
        //}
    }
}
