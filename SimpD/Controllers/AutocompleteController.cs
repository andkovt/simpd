using Microsoft.AspNetCore.Mvc;
using SimpD.Dto.Autocomplete;

namespace SimpD.Controllers;

[ApiController]
[Route("[controller]")]
public class AutocompleteController: ControllerBase
{
    [HttpGet("path")]
    public async Task<ActionResult> GetPath([FromQuery] string path)
    {
        var directories = Directory.EnumerateDirectories(path);
        var response = new List<DirectoryDto>();

        foreach (var directory in directories) {
            var dto = new DirectoryDto() {Name = Path.GetFileName(directory), Path = directory};

            try {
                var subDirectories = Directory.EnumerateDirectories(directory);

                foreach (var subDirectory in subDirectories) {
                    dto.SubDirectories.Add(new DirectoryDto()
                    {
                        Name = Path.GetFileName(subDirectory),
                        Path = subDirectory
                    });
                }
            }
            catch (Exception e) {
                
            }

            response.Add(dto);
        }

        return Ok(response);
    }
}
