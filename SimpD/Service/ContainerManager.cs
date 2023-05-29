using Hangfire;
using Microsoft.EntityFrameworkCore;
using SimpD.Background;
using SimpD.Docker;
using SimpD.Entity;
using SimpD.Enums;

namespace SimpD.Service;

public class ContainerManager
{
    private readonly MainContext context;
    private readonly DockerAdapter dockerAdapter;
    private readonly ILogger<ContainerManager> logger;

    public ContainerManager(MainContext context, DockerAdapter dockerAdapter, ILogger<ContainerManager> logger)
    {
        this.context = context;
        this.dockerAdapter = dockerAdapter;
        this.logger = logger;
    }

    public async Task<Container?> GetAsync(Guid id)
    {
        return await context.Containers
            .Include(c => c.Mounts)
            .Include(c => c.Ports)
            .Include(c => c.EnvironmentVariables)
            .FirstOrDefaultAsync(c => c.Id == id);
    }
    
    public async Task<Container> CreateContainerAsync(Container container)
    {
        logger.LogInformation(
            "Attempting to create a new container with id '{ContainerId}' and name '{Name}'",
            container.Id,
            container.Name);

        if (!container.Image.Contains(":")) {
            container.Image += ":latest";
        }
        
        await context.Containers.AddAsync(container);
        await context.SaveChangesAsync();
        
        logger.LogInformation("Container '{ContainerId}' saved. Queuing create docker job.", container.Id);
        BackgroundJob.Enqueue<CreateDockerContainerJob>(j => j.RunAsync(container.Id));
        
        return container;
    }

    public async Task StartContainerAsync(Container container)
    {
        logger.LogInformation("Starting container {ContainerName}", container.Name);
        await dockerAdapter.StartContainerAsync(container.Name);
        logger.LogInformation("Container {ContainerName} started", container.Name);
    }

    public async Task StopContainerAsync(Container container)
    {
        logger.LogInformation("Stopping container {ContainerName}", container.Name);
        await dockerAdapter.StopContainerAsync(container.Name);
        logger.LogInformation("Container {ContainerName} stopped", container.Name);
    }

    public async Task<Container> RemoveContainerAsync(Guid containerId)
    {
        var entity = await context.Containers.FindAsync(containerId);
        await dockerAdapter.RemoveContainerAsync(entity);

        context.Containers.Remove(entity);
        await context.SaveChangesAsync();

        return entity;
    }

    public async Task<Container> UpdateContainerAsync(Container container)
    {
        var existingEntity = await context.Containers.FindAsync(container.Id);
        if (existingEntity == null) {
            logger.LogError("Attempting to remove non existing container {ContainerName}", container.Name);
            return container;
        }

        await RemoveContainerAsync(existingEntity.Id);
        await CreateContainerAsync(container);

        return container;
    }

    public async Task UpdateStatusAsync(
        Container container,
        ContainerStatus status,
        string? message = null,
        bool save = true)
    {
        var entity = context.Statuses.FirstOrDefault(s => s.Container == container);
        if (entity == null) {
            entity = new Status() {Container = container};
            await context.Statuses.AddAsync(entity);
        }

        entity.ContainerStatus = status;
        entity.Message = message;
        
        if (save) {
            await context.SaveChangesAsync();
        }
    }
}
