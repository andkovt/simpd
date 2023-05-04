namespace SimpD.Dto.Autocomplete;

public class DirectoryDto
{
    public string Name { get; set; }
    public string Path { get; set; }
    public IList<DirectoryDto> SubDirectories { get; set; } = new List<DirectoryDto>();
}
