using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SimpD.Dto;
using SimpD.Entity;
using SimpD.Manager;

namespace SimpD.Controllers;

[ApiController]
[Route("[controller]")]
public class ContainersController : ControllerBase
{
    private readonly DockerConnector dockerConnector;
    private readonly IMapper mapper;
    private readonly ContainerManager containerManager;
    private readonly MainContext context;

    public ContainersController(
        DockerConnector dockerConnector,
        IMapper mapper,
        ContainerManager containerManager,
        MainContext context
    )
    {
        this.dockerConnector = dockerConnector;
        this.mapper = mapper;
        this.containerManager = containerManager;
        this.context = context;
    }
    
    [HttpGet]
    public async Task<ActionResult> ListAsync()
    {
        var entities = context.Containers;
        var containers = mapper.ProjectTo<ContainerViewDto>(entities);
        
        return Ok(containers);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult> ViewAsync(Guid id)
    {
        var entity = await context.Containers.FindAsync(id);
        if (entity == null) {
            return NotFound();
        }
        
        return Ok(mapper.Map<ContainerViewDto>(entity));
    }

    [HttpPost]
    public async Task<ActionResult> CreateAsync([FromBody] ContainerDto containerDto)
    {
        var newEntity = mapper.Map<Container>(containerDto);
        var createdEntity = await containerManager.CreateContainerAsync(newEntity);
        
        return CreatedAtAction("View", new {id = createdEntity.Id}, mapper.Map<ContainerDto>(createdEntity));
    }

    [HttpPut]
    public async Task<IActionResult> EditAsync([FromBody] ContainerDto containerDto)
    {
        return Ok();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync(Guid id)
    {
        var entity = await context.Containers.FindAsync(id);
        if (entity == null) {
            return NotFound();
        }

        context.Containers.Remove(entity);
        await context.SaveChangesAsync();
        
        return Ok(mapper.Map<ContainerViewDto>(entity));
    }
}
