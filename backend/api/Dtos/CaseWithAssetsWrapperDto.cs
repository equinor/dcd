using api.Models;

namespace api.Dtos;

public class CaseWithAssetsWrapperDto
{
    public CaseDto CaseDto { get; set; } = null!;
    public DrainageStrategyDto DrainageStrategyDto { get; set; } = null!;
    public WellProjectDto WellProjectDto { get; set; } = null!;
    public ExplorationDto ExplorationDto { get; set; } = null!;
    public SurfDto SurfDto { get; set; } = null!;
    public SubstructureDto SubstructureDto { get; set; } = null!;
    public TopsideDto TopsideDto { get; set; } = null!;
    public TransportDto TransportDto { get; set; } = null!;
    public WellProjectWellDto[]? WellProjectWellDtos { get; set; }
    public ExplorationWellDto[]? ExplorationWellDto { get; set; }
}
