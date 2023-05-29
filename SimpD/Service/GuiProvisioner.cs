using SimpD.Entity;
using SimpD.Enums;

namespace SimpD.Service;

public class GuiProvisioner
{
    private const string GuiContainerImage = "andkovt/simpd-web:latest";
    private readonly Guid guiContainerId = new Guid("123fa583-0d4f-4dd7-a6e2-8e6c5fcf0d54");
    private readonly ContainerManager containerManager;

    public GuiProvisioner(ContainerManager containerManager)
    {
        this.containerManager = containerManager;
    }
    
    public async Task ProvisionAsync()
    {
        var container = await containerManager.GetAsync(guiContainerId);
        if (container != null) {
            await containerManager.RemoveContainerAsync(container.Id);
        }
        
        container = await CreateGuiContainerAsync();
    }

    private async Task<Container> CreateGuiContainerAsync()
    {
        var container = new Container()
        {
            Id = guiContainerId,
            Image = GuiContainerImage,
            Name = "simpd-web",
            Ports = new List<Port> {new Port(){Container = 3000, Host = 3000, Id = new Guid(), Type = PortType.Tcp}}
        };

        return await containerManager.CreateContainerAsync(container);
    }
}
