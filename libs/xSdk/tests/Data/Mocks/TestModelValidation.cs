using FluentValidation;

namespace xSdk.Data.Mocks
{
    public class TestModelValidation : AbstractValidator<TestModel>
    {
        public TestModelValidation()
        {
            RuleFor(x => x.Name).NotEmpty();
        }
    }
}
