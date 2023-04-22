using System.ComponentModel.DataAnnotations;
using SimpD.Enums;

namespace SimpD.Entity;

public class Mount
{
    public Guid Id { get; set; }
    
    [Required]
    public string Source { get; set; } = "Unknown";
    
    [Required]
    public string Destination { get; set; } = "Unknown";
    
    [Required]
    public MountMode Mode { get; set; }
}
