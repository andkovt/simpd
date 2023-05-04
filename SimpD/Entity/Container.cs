using System.ComponentModel.DataAnnotations;

namespace SimpD.Entity;

public class Container
{
    public Guid Id { get; set; }
    
    [Required]
    public string Name { get; set; } = "Unknown";
    
    [Required]
    public string Image { get; set; } = "Unknown";
    
    public Status? Status { get; set; }

    public IList<EnvironmentVariable> EnvironmentVariables { get; set; } = new List<EnvironmentVariable>();
    public IList<Mount> Mounts { get; set; } = new List<Mount>();
    public IList<Port> Ports { get; set; } = new List<Port>();
}
