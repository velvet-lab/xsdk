using System.ComponentModel.DataAnnotations;

namespace xSdk.Extensions.Variable
{
    public interface ISetup
    {
        ICollection<ValidationResult> Results { get; }

        void Validate();

        void Validate(bool throwIfFails);
    }
}
