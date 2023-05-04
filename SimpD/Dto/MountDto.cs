using System.ComponentModel.DataAnnotations;
using SimpD.Attributes.Validation;

namespace SimpD.Dto;

public class MountDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    public string Source { get; set; } = "";
    [Required]
    public string Destination { get; set; } = "";

    [Required] [ValidMountMode] public string Mode { get; set; } = "ReadWrite";
}
