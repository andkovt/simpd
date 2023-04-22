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
        
        CreateMap<Container, ContainerViewDto>();

        CreateMap<string, MountMode>().ConvertUsing(
            (s, mode) =>
            {
                switch (s) {
                    case "r":
                        return MountMode.ReadOnly;
                    case "rw":
                        return MountMode.ReadWrite;
                    default:
                        throw new Exception($"Unknown mount mode {s}");
                }
            });
    }
}
