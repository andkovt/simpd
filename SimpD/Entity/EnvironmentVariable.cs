using System.ComponentModel.DataAnnotations;

namespace SimpD.Entity;

public class EnvironmentVariable
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "Unknown";
    
    [Required]
    public string Value { get; set; } = "Unknown";
}
