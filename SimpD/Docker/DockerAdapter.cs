using Docker.DotNet;
using Docker.DotNet.Models;
using SimpD.Entity;
using SimpD.Enums;
using SimpD.Exceptions;
using ContainerStatus = SimpD.Enums.ContainerStatus;
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

        parameters.HostConfig = new HostConfig();
        parameters.Env = new List<string>();
        parameters.HostConfig.PortBindings = new Dictionary<string, IList<PortBinding>>();
        parameters.HostConfig.Mounts = new List<global::Docker.DotNet.Models.Mount>();
        
        foreach (var port in container.Ports) {
            var containerPort = port.Container.ToString();
            
            switch (port.Type) {
                case PortType.Tcp:
                    containerPort += "/tcp";
                    break;
                case PortType.Udp:
                    containerPort += "/udp";
                    break;
            }
            
            var portBindings = new List<PortBinding>();
            portBindings.Add(new PortBinding() {HostPort = port.Host.ToString()});
            parameters.HostConfig.PortBindings.Add(containerPort, portBindings);   
        }

        foreach (var mount in container.Mounts) {
            var containerMount = new global::Docker.DotNet.Models.Mount()
            {
                Source = mount.Source,
                Target = mount.Destination,
                Type = "bind",
            };

            if (mount.Mode == MountMode.ReadOnly) {
                containerMount.ReadOnly = true;
            }
            
            parameters.HostConfig.Mounts.Add(containerMount);
        }

        foreach (var environmentVariable in container.EnvironmentVariables) {
            parameters.Env.Add($"{environmentVariable.Name}={environmentVariable.Value}");
        }

        await client.Containers.CreateContainerAsync(parameters);
    }

    public async Task StartContainerAsync(string containerName)
    {
        await client.Containers.StartContainerAsync(containerName, new ContainerStartParameters());
    }

    public async Task StopContainerAsync(string containerName)
    {
        await client.Containers.StopContainerAsync(containerName, new ContainerStopParameters());
    }

    public async Task<IList<Model.ContainerStatus>> GetContainerStatusesAsync()
    {
        var result = new List<Model.ContainerStatus>();
        var response = await client.Containers.ListContainersAsync(new ContainersListParameters() {All = true});

        foreach (var listResponse in response) {
            var name = listResponse.Names[0];
            if (name.StartsWith("/")) {
                name = name.Substring(1);
            }
            
            result.Add(new Model.ContainerStatus() {Name = name, State = listResponse.State, Status = listResponse.Status});
        }
        
        return result;
    }


    public async Task RemoveContainerAsync(Container container)
    {
        try {
            await client.Containers.StopContainerAsync(container.Name, new ContainerStopParameters());
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
