using api.Models;

namespace api.Features.BackgroundServices.ProjectMaster.Dtos;

public class UpdatableFieldsFromProjectMasterDto
{
    public required Guid Id { get; set; }
    public required Guid FusionProjectId { get; set; }
    public required string Name { get; set; }
    public required string CommonLibraryName { get; set; }
    public required string Country { get; set; }
    public ProjectCategory? ProjectCategory { get; set; }
    public ProjectPhase? ProjectPhase { get; set; }
}
