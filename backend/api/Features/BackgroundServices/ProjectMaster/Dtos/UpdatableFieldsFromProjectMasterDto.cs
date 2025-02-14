using api.Models.Enums;

namespace api.Features.BackgroundServices.ProjectMaster.Dtos;

public class UpdatableFieldsFromProjectMasterDto
{
    public required Guid Id { get; set; }
    public required Guid FusionProjectId { get; set; }
    public required string Name { get; set; }
    public required string CommonLibraryName { get; set; }
    public required string Country { get; set; }
    public required ProjectCategory ProjectCategory { get; set; }
    public required ProjectPhase ProjectPhase { get; set; }
}
