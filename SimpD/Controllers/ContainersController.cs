using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SimpD.Dto;
using SimpD.Entity;
using SimpD.Enums;
using SimpD.Service;

namespace SimpD.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainersController : ControllerBase
{
    private readonly IMapper mapper;
    private readonly ContainerManager containerManager;
    private readonly MainContext context;
    private readonly DockerStatusProvider dockerStatusProvider;

    public ContainersController(
        IMapper mapper,
        ContainerManager containerManager,
        MainContext context,
        DockerStatusProvider dockerStatusProvider
    )
    {
        this.mapper = mapper;
        this.containerManager = containerManager;
        this.context = context;
        this.dockerStatusProvider = dockerStatusProvider;
    }
    
    [HttpGet]
    public async Task<ActionResult> ListAsync()
    {
        var containers = context.Containers
            .Include(c => c.Mounts)
            .Include(c => c.EnvironmentVariables)
            .Include(c => c.Ports)
            .Select(e => mapper.Map<ContainerViewDto>(e)).ToList();
        await dockerStatusProvider.ProvideAsync(containers);
        
        return Ok(containers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> ViewAsync(Guid id)
    {
        var entity = context.Containers
            .Include(c => c.Mounts)
            .Include(c => c.Ports)
            .Include(c => c.EnvironmentVariables)
            .FirstOrDefault(c => c.Id == id);
        
        if (entity == null) {
            return NotFound();
        }

        var dto = mapper.Map<ContainerViewDto>(entity);
        await dockerStatusProvider.ProvideAsync(new List<ContainerViewDto>() {dto});
        
        return Ok(dto);
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ContainerDto containerDto)
    {
        var newEntity = mapper.Map<Container>(containerDto);
        var createdEntity = await containerManager.CreateContainerAsync(newEntity);
        
        return CreatedAtAction("View", new {id = createdEntity.Id}, mapper.Map<ContainerDto>(createdEntity));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> EditAsync(Guid id, [FromBody] ContainerDto containerDto)
    {
        var updatedEntity = mapper.Map<Container>(containerDto);
        await containerManager.UpdateContainerAsync(updatedEntity);
        
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var entity = await context.Containers.FindAsync(id);
        if (entity == null) {
            return NotFound();
        }

        await containerManager.RemoveContainerAsync(id);
        
        return Ok(mapper.Map<ContainerViewDto>(entity));
    }

    [HttpPut("{id:guid}/state")]
    public async Task<IActionResult> UpdateStateAsync(Guid id, [FromBody] StateUpdateDto state)
    {
        var entity = await containerManager.GetAsync(id);
        if (entity == null) {
            return NotFound();
        }
        
        switch (state.State) {
            case PreferredState.Started:
                await containerManager.StartContainerAsync(entity);
                break;
            case PreferredState.Stopped:
                await containerManager.StopContainerAsync(entity);
                break;
        }
        
        return Ok();
    }
}
