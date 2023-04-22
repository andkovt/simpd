using System.ComponentModel.DataAnnotations;

namespace SimpD.Attributes.Validation;

public class ValidMountMode : ValidationAttribute
{
    private static readonly string[] ValidModes = new[] {"rw", "r"};
    
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (ValidModes.Contains((string) value!)) {
            return ValidationResult.Success;
        }

        return new ValidationResult($"Unknown mount mode {value}");
    }
}
