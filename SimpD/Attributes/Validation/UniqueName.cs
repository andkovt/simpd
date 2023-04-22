using System.ComponentModel.DataAnnotations;
using SimpD.Dto;

namespace SimpD.Attributes.Validation;

public class UniqueName : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        var dbContext = validationContext.GetService<MainContext>();
        var existingContainer = dbContext?.Containers.FirstOrDefault(c => c.Name == (string) value!);

        if (existingContainer == null) {
            return ValidationResult.Success!;
        }

        var obj = (ContainerDto) validationContext.ObjectInstance;
        if (existingContainer.Id == obj.Id) {
            return ValidationResult.Success!;
        }

        return new ValidationResult("Container must have an unique name");
    }
}
