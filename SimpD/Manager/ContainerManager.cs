using Hangfire;
using SimpD.Background;
using SimpD.Docker;
using SimpD.Entity;
using SimpD.Enums;

namespace SimpD.Manager;

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
        return await context.Containers.FindAsync(id);
    }
    
    public async Task<Container> CreateContainerAsync(Container container)
    {
        logger.LogInformation("Attempting to create a new container with id '{Id}' and name '{Name}'", container.Id, container.Name);
        logger.LogDebug("Container name: '{Name}', Image: '{Image}'", container.Name, container.Image);

        if (!container.Image.Contains(":")) {
            container.Image += ":latest";
        }
        
        await context.Containers.AddAsync(container);
        await context.SaveChangesAsync();

        BackgroundJob.Enqueue<CreateDockerContainerJob>(j => j.RunAsync(container.Id));
        
        return container;
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
