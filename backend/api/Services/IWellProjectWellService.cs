using api.Dtos;
using api.Models;

namespace api.Services;

public interface IWellProjectWellService
{
    Task<ProjectDto> CreateWellProjectWell(WellProjectWellDto wellProjectWellDto);
    Task<WellProjectWellDto[]?> CreateMultipleWellProjectWells(WellProjectWellDto[] wellProjectWellDtos);
    Task<WellProjectWell> GetWellProjectWell(Guid wellId, Guid caseId);
    Task<WellProjectWellDto[]?> CopyWellProjectWell(Guid sourceWellProjectId, Guid targetWellProjectId);
    Task<IEnumerable<WellProjectWell>> GetAll();
}
