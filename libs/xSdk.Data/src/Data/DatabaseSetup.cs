using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using xSdk.Extensions.Variable;

namespace xSdk.Data;

public abstract class DatabaseSetup : IDatabaseSetup
{
    private readonly Dictionary<string, string> _connectionProperties;
    private readonly ICollection<ValidationResult> _validationResults;

    public DatabaseSetup()
    {
        _connectionProperties = new Dictionary<string, string>();
        _validationResults = new Collection<ValidationResult>();
    }

    public IDictionary<string, string> Properties => _connectionProperties;

    public ICollection<ValidationResult> Results => _validationResults;

    public void Initialize() { }

    public void Validate() => Validate(true);

    public void Validate(bool throwIfFails)
    {
        ValidateSetup();

        if (Results != null && Results.Any())
        {
            var validationResult = Results.ValidateResults();
            if (!validationResult && throwIfFails)
            {
                throw new SdkException("Database Setup is not valid");
            }
        }
    }

    protected virtual void ValidateSetup() { }
}
