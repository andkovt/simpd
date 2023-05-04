using System.ComponentModel.DataAnnotations;
using SimpD.Attributes.Validation;

namespace SimpD.Dto;

public class ContainerDto
{
    [Required]
    public Guid Id { get; set; }
    [Required]
    [UniqueName]
    public string Name { get; set; } = "";
    [Required]
    public string Image { get; set; } = "";

    public IList<MountDto> Mounts { get; set; } = new List<MountDto>();
    public IList<EnvironmentVariableDto> EnvironmentVariables { get; set; } = new List<EnvironmentVariableDto>();
    public IList<PortDto> Ports { get; set; } = new List<PortDto>();
}
