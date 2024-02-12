using api.Models;

namespace api.Dtos;

public class CaseWithAssetsWrapperDto
{
    public UpdateCaseDto CaseDto { get; set; } = null!;
    public UpdateDrainageStrategyDto DrainageStrategyDto { get; set; } = null!;
    public UpdateWellProjectDto WellProjectDto { get; set; } = null!;
    public UpdateExplorationDto ExplorationDto { get; set; } = null!;
    public UpdateSurfDto SurfDto { get; set; } = null!;
    public UpdateSubstructureDto SubstructureDto { get; set; } = null!;
    public UpdateTopsideDto TopsideDto { get; set; } = null!;
    public UpdateTransportDto TransportDto { get; set; } = null!;
    public UpdateWellProjectWellDto[]? WellProjectWellDtos { get; set; }
    public UpdateExplorationWellDto[]? ExplorationWellDto { get; set; }
}
