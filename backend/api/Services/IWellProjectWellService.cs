using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectWellService
{
    Task<ProjectDto> CreateWellProjectWell(CreateWellProjectWellDto wellProjectWellDto);
    Task<WellProjectWellDto[]?> CreateMultipleWellProjectWells(CreateWellProjectWellDto[] wellProjectWellDtos);
    Task<WellProjectWell> GetWellProjectWell(Guid wellId, Guid caseId);
    Task<List<WellProjectWell>> GetWellProjectWellsForWellProject(Guid wellProjectId);
    Task<WellProjectWellDto[]?> CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId);
    Task<IEnumerable<WellProjectWell>> GetAll();
}
