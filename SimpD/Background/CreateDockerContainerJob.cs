using SimpD.Docker;
using SimpD.Enums;
using SimpD.Exceptions;
using SimpD.Manager;

namespace SimpD.Background;

public class CreateDockerContainerJob
{
    private readonly ILogger<CreateDockerContainerJob> logger;
    private readonly DockerAdapter dockerAdapter;
    private readonly ContainerManager containerManager;

    public CreateDockerContainerJob(
        ILogger<CreateDockerContainerJob> logger,
        DockerAdapter dockerAdapter,
        ContainerManager containerManager)
    {
        this.logger = logger;
        this.dockerAdapter = dockerAdapter;
        this.containerManager = containerManager;
    }
    
    public async Task RunAsync(Guid containerId)
    {
        logger.LogInformation("Creating docker container for container '{ContainerId}'", containerId);
        var container = await containerManager.GetAsync(containerId);

        if (container == null) {
            logger.LogError("Unable to find container with id '{ContainerId}'", containerId);
            return;
        }
        
        logger.LogInformation("Pulling image for container '{ContainerId}'", containerId);
        await containerManager.UpdateStatusAsync(container, ContainerStatus.PullingImage);
        
        try {
            await dockerAdapter.PullDockerImage(container.Image);
        } catch (ImagePullException e) {
            logger.LogError("Error pulling image for container '{ContainerId}': {Error}", containerId, e.Message);
            await containerManager.UpdateStatusAsync(container, ContainerStatus.Error, e.Message);
            return;
        }

        logger.LogInformation("Image pulled successfully for container '{ContainerId}'", containerId);
        
        logger.LogInformation("Creating docker container {ContainerId}", containerId);
        await dockerAdapter.CreateContainerFromEntityAsync(container);
        await containerManager.UpdateStatusAsync(container, ContainerStatus.Created);
        
        logger.LogInformation("Created docker container {ContainerId}", containerId);
    }
}
