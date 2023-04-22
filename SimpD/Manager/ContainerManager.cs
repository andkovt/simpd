using SimpD.Entity;

namespace SimpD.Manager;

public class ContainerManager
{
    private readonly MainContext context;

    public ContainerManager(MainContext context)
    {
        this.context = context;
    }
    
    public async Task<Container> CreateContainerAsync(Container container)
    {
        await context.Containers.AddAsync(container);
        await context.SaveChangesAsync();
        
        return container;
    }
}
