using api.Models;

namespace api.Dtos;

public class CaseWithAssetsWrapperDto
{
    public APIUpdateCaseDto CaseDto { get; set; } = null!;
    public UpdateDrainageStrategyDto DrainageStrategyDto { get; set; } = null!;
    public UpdateWellProjectDto WellProjectDto { get; set; } = null!;
    public UpdateExplorationDto ExplorationDto { get; set; } = null!;
    public APIUpdateSurfDto SurfDto { get; set; } = null!;
    public APIUpdateSubstructureDto SubstructureDto { get; set; } = null!;
    public APIUpdateTopsideDto TopsideDto { get; set; } = null!;
    public APIUpdateTransportDto TransportDto { get; set; } = null!;
    public UpdateWellProjectWellDto[]? WellProjectWellDtos { get; set; }
    public UpdateExplorationWellDto[]? ExplorationWellDto { get; set; }
}
