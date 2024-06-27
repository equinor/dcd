using api.Models;

namespace api.Dtos;

public class CaseWithAssetsWrapperDto
{
    public APIUpdateCaseWithProfilesDto CaseDto { get; set; } = null!;
    public UpdateDrainageStrategyWithProfilesDto DrainageStrategyDto { get; set; } = null!;
    public UpdateWellProjectWithProfilesDto WellProjectDto { get; set; } = null!;
    public UpdateExplorationWithProfilesDto ExplorationDto { get; set; } = null!;
    public APIUpdateSurfWithProfilesDto SurfDto { get; set; } = null!;
    public APIUpdateSubstructureWithProfilesDto SubstructureDto { get; set; } = null!;
    public APIUpdateTopsideWithProfilesDto TopsideDto { get; set; } = null!;
    public APIUpdateTransportWithProfilesDto TransportDto { get; set; } = null!;
    public UpdateWellProjectWellWithScheduleDto[]? WellProjectWellDtos { get; set; }
    public UpdateExplorationWellDto[]? ExplorationWellDto { get; set; }
}
