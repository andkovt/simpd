using SimpD.Docker;
using SimpD.Dto;

namespace SimpD.Service;

public class DockerStatusProvider
{
    private readonly DockerAdapter dockerAdapter;
    private readonly ILogger<DockerStatusProvider> logger;

    public DockerStatusProvider(DockerAdapter dockerAdapter, ILogger<DockerStatusProvider> logger)
    {
        this.dockerAdapter = dockerAdapter;
        this.logger = logger;
    }
    
    public async Task<IList<ContainerViewDto>> ProvideAsync(IList<ContainerViewDto> containerViews)
    {
        var containerStatuses = await dockerAdapter.GetContainerStatusesAsync();
        
        foreach (var containerView in containerViews) {
            var status = containerStatuses.FirstOrDefault(s => s.Name == containerView.Name);
            if (status == null) {
                logger.LogError("Docker container for container {ContainerName} does not exist", containerView.Name);
                continue;
            }

            containerView.State = status.State;
            containerView.Status = status.Status;
        }
        
        return containerViews;
    }
}
