using System.ComponentModel.DataAnnotations;

using api.Features.Assets.ProjectAssets.DevelopmentOperationalWellCosts.Dtos;
using api.Features.Assets.ProjectAssets.ExplorationOperationalWellCosts.Dtos;
using api.Features.CaseProfiles.Dtos.Well;
using api.Features.Projects.Update;
using api.Features.Wells.Create;
using api.Features.Wells.Update;

namespace api.Features.TechnicalInput.Dtos;

public class UpdateTechnicalInputDto
{
    [Required] public required UpdateProjectDto ProjectDto { get; set; }
    [Required] public required UpdateDevelopmentOperationalWellCostsDto DevelopmentOperationalWellCostsDto { get; set; }
    [Required] public required UpdateExplorationOperationalWellCostsDto ExplorationOperationalWellCostsDto { get; set; }
    [Required] public required List<UpdateWellDto> UpdateWellDtos { get; set; }
    [Required] public required List<CreateWellDto> CreateWellDtos { get; set; }
    [Required] public required List<DeleteWellDto> DeleteWellDtos { get; set; }
}
