using System.ComponentModel.DataAnnotations;
using xSdk.Data.Annotations;

namespace xSdk.Plugin.Tests.Data.Annotations;

public class DataAnnotationAttributeTests
{
    private class IntModel
    {
        [Min(5)]
        public int Count { get; set; }
    }

    private class DoubleModel
    {
        [Min(1.5)]
        public double Value { get; set; }
    }

    private class MaxIntModel
    {
        [Max(100)]
        public int Count { get; set; }
    }

    private class MaxDoubleModel
    {
        [Max(9.9)]
        public double Value { get; set; }
    }

    [Fact]
    public void MinAttribute_Int_AboveMinimum_IsValid()
    {
        var model = new IntModel { Count = 10 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void MinAttribute_Int_BelowMinimum_IsInvalid()
    {
        var model = new IntModel { Count = 3 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void MinAttribute_Int_AtMinimum_IsValid()
    {
        var model = new IntModel { Count = 5 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void MinAttribute_Double_AboveMinimum_IsValid()
    {
        var model = new DoubleModel { Value = 2.5 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void MinAttribute_Double_BelowMinimum_IsInvalid()
    {
        var model = new DoubleModel { Value = 0.5 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void MaxAttribute_Int_BelowMaximum_IsValid()
    {
        var model = new MaxIntModel { Count = 50 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void MaxAttribute_Int_AboveMaximum_IsInvalid()
    {
        var model = new MaxIntModel { Count = 150 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void MaxAttribute_Int_AtMaximum_IsValid()
    {
        var model = new MaxIntModel { Count = 100 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }

    [Fact]
    public void MaxAttribute_Double_AboveMaximum_IsInvalid()
    {
        var model = new MaxDoubleModel { Value = 10.5 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.False(isValid);
    }

    [Fact]
    public void MaxAttribute_Double_BelowMaximum_IsValid()
    {
        var model = new MaxDoubleModel { Value = 5.0 };
        var context = new ValidationContext(model);
        var results = new List<ValidationResult>();

        var isValid = Validator.TryValidateObject(model, context, results, true);

        Assert.True(isValid);
    }
}
