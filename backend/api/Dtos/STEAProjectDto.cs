namespace api.Dtos;

public class STEAProjectDto
{
    public string Name { get; set; } = null!;
    public int StartYear { get; set; }
    public ICollection<STEACaseDto> STEACases { get; set; } = null!;

}