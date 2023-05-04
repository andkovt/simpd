using System.ComponentModel.DataAnnotations;

namespace SimpD.Dto;

public class EnvironmentVariableDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Name { get; set; } = "Unknown";
    [Required]
    public string Value { get; set; } = "Unknown";
}
