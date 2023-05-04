using Docker.DotNet;
using Docker.DotNet.Models;
using SimpD.Entity;
using SimpD.Exceptions;
using Mount = SimpD.Entity.Mount;

namespace SimpD.Docker;

public class DockerAdapter
{
    private readonly DockerClient client;
    
    public DockerAdapter()
    {
        client = new DockerClientConfiguration().CreateClient();
    }

    public async Task PullDockerImage(string image)
    {
        var pullProgress = new PullProgress();
        
        await client.Images.CreateImageAsync(
            new ImagesCreateParameters() {FromImage = image},
            new AuthConfig(),
            pullProgress
        );

        if (pullProgress.LatestError != null) {
            throw new ImagePullException(pullProgress.LatestError);
        }
    }

    public async Task CreateContainerFromEntityAsync(Container container)
    {
        EnsureHostMountPathsExist(container.Mounts);
        
        var parameters = new CreateContainerParameters();
        
        parameters.Name = container.Name;
        parameters.Image = container.Image;

        await client.Containers.CreateContainerAsync(parameters);
    }

    public async Task RemoveContainerAsync(Container container)
    {
        try {
            await client.Containers.RemoveContainerAsync(container.Name, new ContainerRemoveParameters());
        } catch (DockerContainerNotFoundException e) {
            
        }
    }

    private void EnsureHostMountPathsExist(IList<Mount> mounts)
    {
        foreach (var mount in mounts) {
            Directory.CreateDirectory(mount.Source);
        }
    }
}
