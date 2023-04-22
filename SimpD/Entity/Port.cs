using System.ComponentModel.DataAnnotations;
using SimpD.Enums;

namespace SimpD.Entity;

public class Port
{
    public Guid Id { get; set; }
    
    [Required]
    public ushort Host { get; set; }
    [Required]
    public ushort Container { get; set; }
    [Required]
    public PortType Type { get; set; }
}
