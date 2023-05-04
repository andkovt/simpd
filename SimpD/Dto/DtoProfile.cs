using AutoMapper;
using SimpD.Entity;
using SimpD.Enums;

namespace SimpD.Dto;

public class DtoProfile : Profile
{
    public DtoProfile()
    {
        CreateMap<ContainerDto, Container>();
        CreateMap<Container, ContainerDto>();
        
        CreateMap<MountDto, Mount>();
        CreateMap<Mount, MountDto>();

        CreateMap<Port, PortDto>();
        CreateMap<PortDto, Port>();

        CreateMap<EnvironmentVariable, EnvironmentVariableDto>();
        CreateMap<EnvironmentVariableDto, EnvironmentVariable>();
        
        CreateMap<Container, ContainerViewDto>();
    }
}
