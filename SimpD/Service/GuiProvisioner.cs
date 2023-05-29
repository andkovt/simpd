using SimpD.Entity;
using SimpD.Enums;

namespace SimpD.Service;

public class GuiProvisioner
{
    private const string ContainerName = "simpd-web";
    private readonly Guid guiContainerId = new ("123fa583-0d4f-4dd7-a6e2-8e6c5fcf0d54");
    private readonly ContainerManager containerManager;
    private readonly IConfiguration configuration;

    private readonly string image;
    private readonly ushort port;

    public GuiProvisioner(ContainerManager containerManager, IConfiguration configuration)
    {
        this.containerManager = containerManager;
        this.configuration = configuration;

        image = this.configuration["Gui:Image"] ?? "";
        port = ushort.Parse(this.configuration["Gui:port"] ?? "0");
    }
    
    public async Task ProvisionAsync()
    {
        var container = await containerManager.GetAsync(guiContainerId);
        if (container != null) {
            await containerManager.RemoveContainerAsync(container.Id);
        }
        
        await CreateGuiContainerAsync();
    }

    private async Task<Container> CreateGuiContainerAsync()
    {
        var container = new Container()
        {
            Id = guiContainerId,
            Image = image,
            Name = ContainerName,
            
            Ports = new List<Port> {new (){Container = 3000, Host = port, Id = new Guid(), Type = PortType.Tcp}}
        };

        return await containerManager.CreateContainerAsync(container);
    }
}
