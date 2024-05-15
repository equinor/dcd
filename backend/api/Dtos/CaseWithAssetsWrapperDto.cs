using api.Models;

namespace api.Dtos;

public class CaseWithAssetsWrapperDto
{
    public APIUpdateCaseDto CaseDto { get; set; } = null!;
    public UpdateDrainageStrategyWithProfilesDto DrainageStrategyDto { get; set; } = null!;
    public UpdateWellProjectDto WellProjectDto { get; set; } = null!;
    public UpdateExplorationDto ExplorationDto { get; set; } = null!;
    public APIUpdateSurfDto SurfDto { get; set; } = null!;
    public APIUpdateSubstructureWithProfilesDto SubstructureDto { get; set; } = null!;
    public APIUpdateTopsideWithProfilesDto TopsideDto { get; set; } = null!;
    public APIUpdateTransportDto TransportDto { get; set; } = null!;
    public UpdateWellProjectWellDto[]? WellProjectWellDtos { get; set; }
    public UpdateExplorationWellDto[]? ExplorationWellDto { get; set; }
}
