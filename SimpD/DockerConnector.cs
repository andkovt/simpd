using Docker.DotNet;
using Docker.DotNet.Models;

namespace SimpD;

public class DockerConnector
{
    private readonly DockerClient client;
    
    public DockerConnector()
    {
        client = new DockerClientConfiguration().CreateClient();
    }

    public async Task<IList<ContainerListResponse>> ListContainersAsync()
    {
        return await client.Containers.ListContainersAsync(new ContainersListParameters());
    }
}
