using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Projects.Update;
using api.Features.Wells.Create;
using api.Features.Wells.Update;

namespace api.Features.TechnicalInput.Dtos;

public class UpdateTechnicalInputDto
{
    public UpdateDevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; } = null!;
    public UpdateExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; } = null!;
    public UpdateWellDto[]? UpdateWellDtos { get; set; }
    public CreateWellDto[]? CreateWellDtos { get; set; }
    public DeleteWellDto[]? DeleteWellDtos { get; set; }
    public UpdateProjectDto ProjectDto { get; set; } = null!;
}
