namespace api.Features.Stea.Dtos;

public class SteaProjectDto
{
    public required string Name { get; set; }
    public required int StartYear { get; set; }
    public required List<SteaCaseDto> SteaCases { get; set; }
}
