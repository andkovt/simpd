using System.ComponentModel.DataAnnotations;
using SimpD.Enums;

namespace SimpD.Dto;

public class PortDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public ushort Host { get; set; }
    [Required]
    public ushort Container { get; set; }
    [Required]
    public string Type { get; set; }
}
