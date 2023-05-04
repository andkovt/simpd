using SimpD.Enums;

namespace SimpD.Entity;

public class Status
{
    public Guid Id { get; set; }
    public Container? Container { get; set; }
    public ContainerStatus ContainerStatus { get; set; }
    public string? Message { get; set; }
}
